using Microsoft.AspNetCore.Mvc;
using PokeCharts.Controllers;
using PokeCharts.Daos;
using PokeCharts.Extensions.Microsoft.AspNetCore.Diagnostics;
using PokeCharts.Extensions.Microsoft.AspNetCore.Mvc;
using PokeCharts.Extensions.Microsoft.Extensions.DependencyInjection;
using PokeCharts.Filters;
using PokeCharts.Handlers.Exceptions;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace PokeCharts;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPokemonDao, PokemonDao>()
            .AddSingleton<IMoveDao, MoveDao>()
            .AddSingleton<IPokemonTypeDao, PokemonTypeDao>()
            .AddSingleton<IPokemonMoveDao, PokemonMoveDao>()
            .AddSingleton<IModelExceptionHandler, MultipleModelExceptionHandlers>()
            .AddSingleton<ModelExceptionFilterAttribute>()
            .AddSingleton<SystemExceptionFilterAttribute>();

        services.AddControllers(options =>
        {
            options.Filters.AddService(typeof(ModelExceptionFilterAttribute));
            options.Filters.AddService(typeof(SystemExceptionFilterAttribute));
        }).ConfigureApiBehaviorOptions(options => options.ConfigureClientErrorMapping());

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

        app.UseStatusCodePages(async ctx => await ctx.WriteProblemDetailsAsJsonAsync());
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();
        // Add custom middlewares here, between UseAuthorization and UseEndpoints
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}