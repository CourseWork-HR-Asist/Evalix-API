using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common;
using Application.Common.Claims;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Application.Users.Models;
using Domain.RefreshTokens;
using Domain.Roles;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public record UserLoginWithGoogleCommand: IRequest<Result<AuthResult, UserException>>
{
    public required string Token { get; init; }
}

public class LoginWithGoogleCommandHandler(
    IUserRepository userRepository,
    IUserQueries userQueries,
    ITokenGenerator tokenGenerator,
    IPasswordHasher passwordHasher,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    IRoleQueries roleQueries)
    : IRequestHandler<UserLoginWithGoogleCommand, Result<AuthResult, UserException>>
{
    public async Task<Result<AuthResult, UserException>> Handle(UserLoginWithGoogleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var googleToken = jwtHandler.ReadJwtToken(request.Token); 
            
            var email = googleToken.Claims.FirstOrDefault(c => c.Type == GoogleClaimTypes.Email)?.Value;
       
            var user = await userQueries.SearchByEmail(email, cancellationToken);
            return await user.Match(
                async u => await LoginUser(u, cancellationToken),
                async () =>
                {
                    var role = await roleQueries.SearchByTitle("Recruiter", cancellationToken);
                    return await role.Match(
                        async r =>
                        {
                            var firstName = googleToken.Claims.FirstOrDefault(c => c.Type == GoogleClaimTypes.GivenName)?.Value;
                            var lastName = googleToken.Claims.FirstOrDefault(c => c.Type == GoogleClaimTypes.FamilyName)?.Value;
                            var username = googleToken.Claims.FirstOrDefault(c => c.Type == GoogleClaimTypes.Name)?.Value;
                       
                            return await RegisterUserByGoogleCredentials(email, firstName, lastName, username, r, cancellationToken);
                        },
                        () => Task.FromResult<Result<AuthResult, UserException>>(new UserRoleNotFoundException("User")));
                }
            );
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }
    
    private async Task<Result<AuthResult, UserException>> LoginUser(User user, CancellationToken cancellationToken)
    {
        try
        {
            var token = tokenGenerator.GenerateToken(user);
            var refreshToken =
                await refreshTokenRepository.Add(refreshTokenGenerator.Generate(user.Id), CancellationToken.None);

            return new AuthResult(token, refreshToken.Token);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }
    private async Task<Result<AuthResult, UserException>> RegisterUserByGoogleCredentials(string email, string firstName, string lastName, string username, Role role, CancellationToken cancellationToken)
    {
        try
        {
            
            var entity = User.New(
                UserId.New(),
                firstName,
                email,
                passwordHasher.HashPassword(Guid.NewGuid().ToString()),
                role.Id);
       
            var user = await userRepository.Add(entity, cancellationToken);
            var token = tokenGenerator.GenerateToken(user);
            var refreshToken =
                await refreshTokenRepository.Add(refreshTokenGenerator.Generate(user.Id), CancellationToken.None);
            return new AuthResult(token, refreshToken.Token);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
        
    }
}
