using Domain.Vacancies;
using Domain.VacancySkills;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IVacancySkillQueries
{
    Task<IReadOnlyList<VacancySkill>> GetAll(CancellationToken cancellationToken);
    Task<Option<VacancySkill>> GetById (VacancySkillId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<VacancySkill>> GetByVacancyId(VacancyId id, CancellationToken cancellationToken);
}