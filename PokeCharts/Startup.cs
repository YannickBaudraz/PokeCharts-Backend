using System.ComponentModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using PokeCharts.Controllers;
using PokeCharts.Dao;
using PokeCharts.Filters;
using PokeCharts.Handlers.Exceptions;
using static PokeCharts.Constants.RfcLink.Http.StatusCode;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace PokeCharts;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPokemonDao, PokemonDao>()
                .AddSingleton<IModelExceptionHandler, MultipleModelExceptionHandlers>()
                .AddSingleton<ModelExceptionFilterAttribute>()
                .AddSingleton<SystemExceptionFilterAttribute>();

        IMvcBuilder mvcBuilder = services.AddControllers(options =>
        {
            options.Filters.AddService(typeof(ModelExceptionFilterAttribute));
            options.Filters.AddService(typeof(SystemExceptionFilterAttribute));
        });

        mvcBuilder.ConfigureApiBehaviorOptions(ConfigureClientErrorMapping);

        services.AddProblemDetails()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(ErrorController.BaseRoute);
        }

        app.UseStatusCodePages(async ctx => await WriteProblemDetailsAsJsonAsync(ctx));
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private static async Task WriteProblemDetailsAsJsonAsync(StatusCodeContext statusCodeContext)
    {
        int responseStatusCode = statusCodeContext.HttpContext.Response.StatusCode;
        if (responseStatusCode is < 400 or >= 600)
            return;

        ProblemDetails? problemDetails = Extensions.ProblemDetails.From(statusCodeContext);
        if (problemDetails is null)
            return;

        statusCodeContext.HttpContext.Response.ContentType = "application/problem+json; charset=utf-8";
        await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(problemDetails);
    }

    private static void ConfigureClientErrorMapping(ApiBehaviorOptions options)
    {
        IEnumerable<int> clientErrorCodes = Enum.GetValues<ClientError>().Cast<int>();
        IEnumerable<int> serverErrorCodes = Enum.GetValues<ServerError>().Cast<int>();
        IEnumerable<int> errorCodes = clientErrorCodes.Concat(serverErrorCodes);

        errorCodes.ToList()
                  .ForEach(errorCode =>
                  {
                      if (!options.ClientErrorMapping.TryGetValue(errorCode, out _))
                          options.ClientErrorMapping.Add(errorCode, new ClientErrorData());

                      bool clientErrorParsed = TryParseClientError(errorCode, out ClientError clientError);
                      bool serverErrorParsed = TryParseServerError(errorCode, out ServerError serverError);

                      if (!clientErrorParsed && !serverErrorParsed)
                          return;

                      Enum enumError = clientErrorParsed
                          ? clientError
                          : serverError;

                      string? description = enumError.GetAttributeOfType<DescriptionAttribute>()?.Description;
                      options.ClientErrorMapping[errorCode].Link = description;
                      options.ClientErrorMapping[errorCode].Title = enumError.GetDisplayName();
                  });
    }
}