using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Skills;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class SkillRepository(ApplicationDbContext context) : ISkillRepository, ISkillQueries
{
    public async Task<Skill> Add(Skill skill, CancellationToken cancellationToken)
    {
        await context.Skills.AddAsync(skill, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return skill;
    }

    public async Task<Skill> Update(Skill skill, CancellationToken cancellationToken)
    {
        context.Skills.Update(skill);

        await context.SaveChangesAsync(cancellationToken);

        return skill;
    }

    public async Task<Skill> Delete(Skill skill, CancellationToken cancellationToken)
    {
        context.Skills.Remove(skill);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return skill;
    }

    public async Task<IReadOnlyList<Skill>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Skills
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Skill>> GetById(SkillId id, CancellationToken cancellationToken)
    {
        var entity = await context.Skills
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Skill>() : Option.Some(entity);
    }

    public async Task<Option<Skill>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Skills
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == name, cancellationToken);

        return entity == null ? Option.None<Skill>() : Option.Some(entity);
    }
}