using Domain.Vacancies;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IVacancyQueries
{
    Task<IReadOnlyList<Vacancy>> GetAll (CancellationToken cancellationToken);
    Task<Option<Vacancy>> Get(VacancyId id, CancellationToken cancellationToken);
    Task<Option<Vacancy>> GetByTitle(string title, CancellationToken cancellationToken);
}