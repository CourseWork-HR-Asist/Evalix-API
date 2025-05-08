using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Roles.Commands;
using Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("roles/v1/[controller]")]
[ApiController]
public class RolesController(ISender sender, IRoleQueries roleQueries) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await roleQueries.GetAll(cancellationToken);

        return entities.Select(RoleDto.FromDomainModel).ToList();
    }
    
    [HttpGet("[action]/{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> GetById([FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var entity = await roleQueries.GetById(new RoleId(roleId), cancellationToken);    

        return entity.Match<ActionResult<RoleDto>>(r => RoleDto.FromDomainModel(r), () => NotFound());
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<RoleDto>> Create([FromBody] RoleCreateDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateRoleCommand
        {
            Title = request.Title
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
    
    [HttpPut("[action]/{roleId:guid}")] 
    public async Task<ActionResult<RoleDto>> Update([FromRoute] Guid roleId, [FromBody] RoleUpdateDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateRoleCommand
        {
            RoleId = roleId,
            Title = request.Title
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("[action]/{roleId:guid}")] 
    public async Task<ActionResult<RoleDto>> Delete([FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var input = new DeleteRoleCommand
        {
            RoleId = roleId
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
}