using Application.VacancySkills.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class VacancySkillErrorHandler
{
    public static ObjectResult ToObjectResult(this VacancySkillException exception) => new ObjectResult(exception.Message)
    {
        StatusCode = exception switch
        {
            VacancySkillNotFoundException
                or SkillForVacancyNotFoundException
                or VacancyForSkillNotFoundException
                => StatusCodes.Status404NotFound,
            VacancySkillAlreadyExistsException => StatusCodes.Status409Conflict,
            VacancySkillUnknownException => StatusCodes.Status500InternalServerError,
            _ => throw new NotImplementedException("VacancySkill error handler is not implemented")
        }
    };
}