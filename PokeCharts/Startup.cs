using Microsoft.AspNetCore.Mvc;
using PokeCharts.Controllers;
using PokeCharts.Daos;
using PokeCharts.Extensions;
using PokeCharts.Filters;
using PokeCharts.Handlers.Exceptions;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace PokeCharts;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPokemonDao, PokemonDao>()
            .AddSingleton<IModelExceptionHandler, MultipleModelExceptionHandlers>()
            .AddSingleton<ModelExceptionFilterAttribute>()
            .AddSingleton<SystemExceptionFilterAttribute>();

        services.AddControllers(options =>
        {
            options.Filters.AddService(typeof(ModelExceptionFilterAttribute));
            options.Filters.AddService(typeof(SystemExceptionFilterAttribute));
        }).ConfigureApiBehaviorOptions(StartupConfigurationHelper.ConfigureClientErrorMapping);

        string[] allowedOrigins = Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        services.AddCors(allowedOrigins)
            .AddProblemDetails()
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

        app.UseStatusCodePages(async ctx => await StartupConfigurationHelper.WriteProblemDetailsAsJsonAsync(ctx));
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();
        // Add custom middlewares here, between UseAuthorization and UseEndpoints
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}