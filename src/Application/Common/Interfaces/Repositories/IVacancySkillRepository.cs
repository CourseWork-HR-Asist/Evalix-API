using Domain.VacancySkills;

namespace Application.Common.Interfaces.Repositories;

public interface IVacancySkillRepository
{
    Task<VacancySkill> Add (VacancySkill vacancySkill, CancellationToken cancellationToken);
    Task<VacancySkill> Delete (VacancySkill vacancySkill, CancellationToken cancellationToken);
    Task<VacancySkill> Update (VacancySkill vacancySkill, CancellationToken cancellationToken);
}