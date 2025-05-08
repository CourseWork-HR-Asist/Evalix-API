using Application.Skills.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class SkillErrorHandler
{
    public static ObjectResult ToObjectResult(this SkillExceptions exception) => new ObjectResult(exception.Message)
    {
        StatusCode = exception switch
        {
            SkillAlreadyExistsException => StatusCodes.Status409Conflict,
            SkillNotFoundException => StatusCodes.Status404NotFound,
            SkillUnknownException => StatusCodes.Status500InternalServerError,
            _ => throw new NotImplementedException("Skill error handler is not implemented")
        }
    };
}