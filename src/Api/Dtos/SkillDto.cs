using Domain.Skills;

namespace Api.Dtos;

public record SkillCreateDto(string Title)
{
    public static SkillCreateDto FromDomainModel(Skill skill) => new (skill.Title);
}

public record SkillDto(
    Guid? Id,
    string Title)
{
    public static SkillDto FromDomainModel(Skill skill)
        => new(
            Id: skill.Id.Value,
            Title: skill.Title);
}

public record SkillUpdateDto(Guid id, string? Title)
{
    public static RoleUpdateDto FromDomainModel(Skill role) => new(role.Id.Value, role.Title);
}