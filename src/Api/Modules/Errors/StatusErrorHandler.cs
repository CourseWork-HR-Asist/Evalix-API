using Application.Statuses.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class StatusErrorHandler
{
    public static ObjectResult ToObjectResult(this StatusExceptions exception) => new ObjectResult(exception.Message)
    {
        StatusCode = exception switch
        {
            StatusAlreadyExistsException => StatusCodes.Status409Conflict,
            StatusNotFoundException => StatusCodes.Status404NotFound,
            StatusUnknownException => StatusCodes.Status500InternalServerError,
            _ => throw new NotImplementedException("Status error handler is not implemented")
        }
    };
}