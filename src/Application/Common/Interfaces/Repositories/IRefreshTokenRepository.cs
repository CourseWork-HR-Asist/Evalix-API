using Domain.RefreshTokens;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<Option<RefreshToken>> GetByToken(string token, CancellationToken cancellationToken);
    Task<IReadOnlyList<RefreshToken>> GetTokensByUserId(UserId userId, CancellationToken cancellationToken);
    Task<Option<RefreshToken>> Get(RefreshTokenId id, CancellationToken cancellationToken);
    
    Task<RefreshToken> Add (RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken> Update (RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken> Delete (RefreshToken refreshToken, CancellationToken cancellationToken);
}