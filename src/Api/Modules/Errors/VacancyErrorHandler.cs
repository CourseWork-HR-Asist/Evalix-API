using Application.Vacancies.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class VacancyErrorHandler
{
    public static ObjectResult ToObjectResult(this VacancyException exception) => new ObjectResult(exception.Message)
    {
        StatusCode = exception switch
        {
            VacancyAlreadyExistsException => StatusCodes.Status409Conflict,
            VacancyNotFoundException
                or UserNotFoundException
                or SkillNotFoundException => StatusCodes.Status404NotFound,
            VacancyUnknownException => StatusCodes.Status500InternalServerError,
            _ => throw new NotImplementedException("Vacancy error handler is not implemented")
        }
    };
}