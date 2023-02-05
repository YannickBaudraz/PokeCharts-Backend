using Microsoft.AspNetCore.Mvc;
using PokeCharts.Controllers;
using PokeCharts.Dao;
using PokeCharts.Filters;
using PokeCharts.Handlers.Exceptions;

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

        services.AddControllers(options =>
        {
            options.Filters.AddService(typeof(ModelExceptionFilterAttribute));
            options.Filters.AddService(typeof(SystemExceptionFilterAttribute));
        });

        services.AddProblemDetails();

        services.AddEndpointsApiExplorer()
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

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}