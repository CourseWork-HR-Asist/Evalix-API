using Application.Evaluations.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class EvaluationsErrorHandler
{
    public static ObjectResult ToObjectResult(this EvaluationExceptions exception) =>
        new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                EvaluationNotFoundException
                    or EvaluationResumeNotFoundException
                    or EvaluationStatusNotFoundException
                    or EvaluationVacancyNotFoundException => StatusCodes.Status404NotFound,
                EvaluationAlreadyExistsException => StatusCodes.Status409Conflict,
                EvaluationUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Evaluation error handler is not implemented")
            }
        };
}