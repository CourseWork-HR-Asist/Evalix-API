using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("users/v1/[controller]")]
[ApiController]
public class UserController(ISender sender, IUserQueries userQueries) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await userQueries.GetAll(cancellationToken);

        return entities.Select(UserDto.FromDomainModel).ToList();
    }

    
    [HttpPost("google-auth/[action]")]
    public async Task<ActionResult<TokenDto>> LoginWithGoogle([FromBody] GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        var input = new UserLoginWithGoogleCommand
        {
            Token = request.Token
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TokenDto>>(
            u =>
            {
                Response.Cookies.Append("refresh_token", u.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
                return Ok(new TokenDto(u.Token));
            },
            e => e.ToObjectResult());
    }
}