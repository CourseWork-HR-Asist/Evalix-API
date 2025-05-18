using Domain.Skills;
using Domain.Vacancies;
using Domain.VacancySkills;

namespace Api.Dtos;

public record VacancySkillDto(
    Guid Id,
    Guid VacancyId,
    VacancyDto Vacancy,
    Guid SkillId,
    SkillDto Skill,
    int Level,
    int Experience)
{
    public static VacancySkillDto FromDomainModel(VacancySkill vacancySkill) => new(
        Id: vacancySkill.Id.Value,
        VacancyId: vacancySkill.VacancyId.Value,
        Vacancy: vacancySkill.Vacancy is null ? null : VacancyDto.FromDomainModel(vacancySkill.Vacancy),
        SkillId: vacancySkill.SkillId.Value,
        Skill: vacancySkill.Skill is null ? null : SkillDto.FromDomainModel(vacancySkill.Skill),
        Level: vacancySkill.Level,
        Experience: vacancySkill.Experience
    );
}

public record VacancySkillCreateDto(
    Guid VacancyId,
    Guid SkillId,
    int Level,
    int Experience);

public record VacancySkillCreateWithVacancyDto(
    Guid SkillId,
    int Level,
    int Experience)
{
    public VacancySkill ToDomain()
    {
        return VacancySkill.New(
            VacancySkillId.New(),
            VacancyId.Empty(),
            new SkillId(SkillId),
            Level,
            Experience
        );
    }
}
    