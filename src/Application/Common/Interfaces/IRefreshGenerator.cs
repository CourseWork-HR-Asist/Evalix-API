using Domain.RefreshTokens;
using Domain.Users;

namespace Application.Common.Interfaces;

public interface IRefreshTokenGenerator
{
    public RefreshToken Generate(UserId userId);
}