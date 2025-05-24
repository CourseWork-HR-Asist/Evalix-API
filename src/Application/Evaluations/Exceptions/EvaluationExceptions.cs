using Domain.Evaluations;
using Domain.Resumes;
using Domain.Statuses;
using Domain.Vacancies;

namespace Application.Evaluations.Exceptions;

public class EvaluationExceptions(EvaluationId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public EvaluationId EvaluationId { get; } = id;
}

public class EvaluationNotFoundException(EvaluationId id) : EvaluationExceptions(id, "Evaluation not found");

public class EvaluationAlreadyExistsException(EvaluationId id) : EvaluationExceptions(id, "Evaluation already exists");

public class EvaluationStatusNotFoundException(StatusId id) : EvaluationExceptions(EvaluationId.Empty(), $"Status with id: {id} not found");

public class EvaluationVacancyNotFoundException(VacancyId id) : EvaluationExceptions(EvaluationId.Empty(), $"Vacancy with id: {id} not found");

public class EvaluationResumeNotFoundException(ResumeId id) : EvaluationExceptions(EvaluationId.Empty(), $"Resume with id: {id} not found");

public class EvaluationUnknownException(EvaluationId id, Exception innerException)
    : EvaluationExceptions(id, "Unknown exception", innerException);