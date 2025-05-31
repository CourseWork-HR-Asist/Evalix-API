using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Users;
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
            .Include(x => x.Recruiter)
            .Include(x => x.VacancySkills)
                .ThenInclude(s => s.Skill)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Vacancy>> GetById(VacancyId id, CancellationToken cancellationToken)
    {
        var entity = await context.Vacancies
            .AsNoTracking()
            .Include(x => x.Recruiter)
            .Include(x => x.VacancySkills)
                .ThenInclude(s => s.Skill)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        Console.WriteLine(entity.ToLLMString());

        return entity == null ? Option.None<Vacancy>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Vacancy>> GetByUserId(UserId userId, CancellationToken cancellationToken)
    {
        return  await context.Vacancies
            .AsNoTracking()
            .Include(x => x.Recruiter)
            .Include(x => x.VacancySkills)
                .ThenInclude(s => s.Skill)
            .Where(x => x.RecruiterId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Vacancy>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Vacancies
            .AsNoTracking()
            .Include(x => x.Recruiter)
            .Include(x => x.VacancySkills)
                .ThenInclude(s => s.Skill)
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return entity == null ? Option.None<Vacancy>() : Option.Some(entity);
    }
}