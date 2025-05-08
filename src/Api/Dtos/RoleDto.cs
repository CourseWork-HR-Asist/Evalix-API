using Domain.Roles;

namespace Api.Dtos;

public record RoleCreateDto(string Title)
{
    public static RoleCreateDto FromDomainModel(Role role) => new(role.Title);
}
public record RoleDto(
    Guid? Id,
    string Title)
{
    public static RoleDto FromDomainModel(Role role)
        => new(
            Id: role.Id.Value,
            Title: role.Title);
}

public record RoleUpdateDto(Guid id, string? Title)
{
    public static RoleUpdateDto FromDomainModel(Role role) => new(role.Id.Value, role.Title);
}