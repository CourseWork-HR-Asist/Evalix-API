using Domain.Users;

namespace Api.Dtos;

public record UserDto(
    Guid? Id,
    string? FirstName,
    string? Email,
    string? Username,
    DateTime? UpdatedAt,
    Guid? RoleId,
    RoleDto? Role)
{
    public static UserDto FromDomainModel(User user)
        => new(
            Id: user.Id.Value,
            FirstName: user.FirstName,
            Email: user.Email,
            Username: user.FirstName,
            UpdatedAt: user.UpdatedAt,
            RoleId: user.RoleId.Value,
            Role: user.Role is null ? null : RoleDto.FromDomainModel(user.Role));
}


public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Password
);

public record UserUpdateRoleDto(
    Guid RoleId,
    Guid UserId
);

public record TokenDto(
    string Token
);

public record RefreshTokenDto(
    Guid UserId,
    string RefreshToken
);

public record GoogleLoginRequest(
    string Token
);