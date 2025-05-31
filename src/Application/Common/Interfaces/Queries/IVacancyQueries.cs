using Domain.Users;
using Domain.Vacancies;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IVacancyQueries
{
    Task<IReadOnlyList<Vacancy>> GetAll (CancellationToken cancellationToken);
    Task<Option<Vacancy>> GetById(VacancyId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Vacancy>> GetByUserId(UserId userId, CancellationToken cancellationToken);
    Task<Option<Vacancy>> GetByTitle(string title, CancellationToken cancellationToken);
}