using Domain.Skills;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ISkillQueries
{
    Task<IReadOnlyList<Skill>> GetAll (CancellationToken cancellationToken);
    Task<Option<Skill>> GetById(SkillId id, CancellationToken cancellationToken);
    Task<Option<Skill>> GetByName(string name, CancellationToken cancellationToken);
}