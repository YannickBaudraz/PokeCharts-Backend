using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeCharts.Exceptions.Handlers.Exceptions;
using MvcExtensions = PokeCharts.Extensions.Microsoft.AspNetCore.Mvc;

namespace PokeCharts.Exceptions.Filters;

public class ModelExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IModelExceptionHandler _handler;

    public ModelExceptionFilterAttribute(IModelExceptionHandler handler)
    {
        _handler = handler;
    }

    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not ModelException modelException || _handler.Handle(modelException) is not { } httpStatusCode)
            return;

        ProblemDetails problemDetails = MvcExtensions.ProblemDetails.From(context, httpStatusCode);
        context.Result = new ObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json; charset=utf-8" },
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
}