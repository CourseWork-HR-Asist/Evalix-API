using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Vacancies;
using Domain.VacancySkills;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class VacancySkillRepository(ApplicationDbContext context): IVacancySkillRepository, IVacancySkillQueries
{
    public async Task<VacancySkill> Add(VacancySkill vacancySkill, CancellationToken cancellationToken)
    {
        await context.VacancySkills.AddAsync(vacancySkill, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return vacancySkill;
    }

    public async Task<VacancySkill> Delete(VacancySkill vacancySkill, CancellationToken cancellationToken)
    {
        context.VacancySkills.Remove(vacancySkill);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return vacancySkill;
    }

    public async Task<IReadOnlyList<VacancySkill>> GetAll(CancellationToken cancellationToken)
    {
        return await context.VacancySkills
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<VacancySkill>> GetById(VacancySkillId id, CancellationToken cancellationToken)
    {
        var entity = await context.VacancySkills
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return entity == null ? Option.None<VacancySkill>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<VacancySkill>> GetByVacancyId(VacancyId id, CancellationToken cancellationToken)
    {
        var entity = await context.VacancySkills
            .AsNoTracking()
            .Where(x => x.VacancyId == id)
            .ToListAsync(cancellationToken);
        
        return entity;
    }
}