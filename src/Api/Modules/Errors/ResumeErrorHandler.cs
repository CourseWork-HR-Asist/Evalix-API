using Application.Resumes.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ResumeErrorHandler
{
    public static ObjectResult ToObjectResult(this ResumeExceptions exception) => new ObjectResult(exception.Message)
    {
        StatusCode = exception switch
        {
            ResumeNotFoundException
                or UserForResumeNotFoundException => StatusCodes.Status404NotFound,
            ResumeAlreadyExistsException => StatusCodes.Status409Conflict,
            ResumeUnknownException => StatusCodes.Status500InternalServerError,
            _ => throw new NotImplementedException("Resume error handler is not implemented")
        }
    };
}