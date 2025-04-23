namespace Domain.Skills;

public record SkillId(Guid Value)
{
    public static SkillId New() => new(Guid.NewGuid());
    public static SkillId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}