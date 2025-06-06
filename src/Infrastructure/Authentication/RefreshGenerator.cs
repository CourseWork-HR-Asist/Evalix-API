﻿using System.Security.Cryptography;
using Application.Common.Interfaces;
using Domain.RefreshTokens;
using Domain.Users;

namespace Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private const int TokenLength = 64;

    public RefreshToken Generate(UserId userId)
    {
        var id = RefreshTokenId.New();
        var token = GenerateSecureToken();
        var expires = DateTime.UtcNow.AddDays(7);
        
        return RefreshToken.New(id, userId, token, expires);
    }
    
    private static string GenerateSecureToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(TokenLength);
        return Convert.ToBase64String(randomBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}