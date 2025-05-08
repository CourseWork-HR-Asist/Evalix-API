using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class VacancyRepository(ApplicationDbContext context): IVacancyRepository, IVacancyQueries
{
    public async Task<Vacancy> Add(Vacancy vacancy, CancellationToken cancellationToken)
    {
        context.Vacancies.Add(vacancy);

        await context.SaveChangesAsync(cancellationToken);

        return vacancy;
    }

    public async Task<Vacancy> Update(Vacancy vacancy, CancellationToken cancellationToken)
    {
        context.Vacancies.Update(vacancy);

        await context.SaveChangesAsync(cancellationToken);

        return vacancy;
    }

    public async Task<Vacancy> Delete(Vacancy vacancy, CancellationToken cancellationToken)
    {
        context.Vacancies.Remove(vacancy);

        await context.SaveChangesAsync(cancellationToken);

        return vacancy;
    }

    public async Task<IReadOnlyList<Vacancy>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Vacancies
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Vacancy>> Get(VacancyId id, CancellationToken cancellationToken)
    {
        var entity = await context.Vacancies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Vacancy>() : Option.Some(entity);
    }

    public async Task<Option<Vacancy>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Vacancies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return entity == null ? Option.None<Vacancy>() : Option.Some(entity);
    }
}