using Domain.Skills;

namespace Application.Common.Interfaces.Repositories;

public interface ISkillRepository
{
    Task<Skill> Add (Skill skill, CancellationToken cancellationToken);
    Task<Skill> Update (Skill skill, CancellationToken cancellationToken);
    Task<Skill> Delete (Skill skill, CancellationToken cancellationToken);
}