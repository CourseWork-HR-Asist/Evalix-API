using Domain.Vacancies;

namespace Application.Common.Interfaces.Repositories;

public interface IVacancyRepository
{
    Task<Vacancy> Add(Vacancy vacancy, CancellationToken cancellationToken);
    Task<Vacancy> Update(Vacancy vacancy, CancellationToken cancellationToken);
    Task<Vacancy> Delete(Vacancy vacancy, CancellationToken cancellationToken);
}