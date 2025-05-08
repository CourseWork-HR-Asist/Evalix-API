using Domain.Evaluations;
using Domain.Resumes;
using Domain.Vacancies;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IEvaluationQueries
{
    Task<IReadOnlyList<Evaluation>> GetAll (CancellationToken cancellationToken);
    Task<Option<Evaluation>> GetById(EvaluationId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Evaluation>> GetByVacancyId (VacancyId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Evaluation>> GetByResumeId (ResumeId id, CancellationToken cancellationToken);
}