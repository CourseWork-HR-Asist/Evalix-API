namespace Domain.VacancySkills;

public record VacancySkillId(Guid Value)
{
    public static VacancySkillId New() => new(Guid.NewGuid());
    public static VacancySkillId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}