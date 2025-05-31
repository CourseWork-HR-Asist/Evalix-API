using Domain.Skills;
using Domain.Vacancies;

namespace Domain.VacancySkills;

public class VacancySkill
{
    public VacancySkillId Id { get; }
    public VacancyId VacancyId { get; private set; }
    public Vacancy? Vacancy { get; private set; }
    public SkillId SkillId { get; private set; }
    public Skill? Skill { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }

    private VacancySkill(VacancySkillId id, VacancyId vacancyId, SkillId skillId, int level, int experience)
    {
        Id = id;
        VacancyId = vacancyId;
        SkillId = skillId;
        Level = level;
        Experience = experience;
    }

    public static VacancySkill New(VacancySkillId id, VacancyId vacancyId, SkillId skillId, int level, int experience) 
        => new(id, vacancyId, skillId, level, experience);
    
    public void UpdateDetails(int level, int experience)
    {
        Level = level;
        Experience = experience;
    }
}