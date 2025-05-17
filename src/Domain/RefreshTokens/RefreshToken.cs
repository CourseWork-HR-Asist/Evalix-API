using Domain.Users;

namespace Domain.RefreshTokens;

public class RefreshToken
{
    public RefreshTokenId Id { get; }
    public string Token { get; private set; }
    public bool Revoked { get; private set; }
    public UserId UserId { get; private set; }
    public User? User { get; }
    public DateTime Expires { get; private set; }
    public DateTime CreatedAt { get; private set; }


    private RefreshToken(RefreshTokenId id, UserId userId, string token, DateTime expires, DateTime createdAt)
    {
        Id = id;
        Token = token;
        UserId = userId;
        Expires = expires;
        CreatedAt = createdAt;
    }

    public static RefreshToken New(RefreshTokenId id, UserId userId, string token, DateTime expired) =>
        new RefreshToken(id, userId, token, expired, DateTime.UtcNow);
    
    public void Revoke() => Revoked = true;
}