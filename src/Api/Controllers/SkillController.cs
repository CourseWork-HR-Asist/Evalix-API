using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Skills.Commands;
using Domain.Skills;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("skills/v1/[controller]")]
[ApiController]
public class SkillController(ISender sender, ISkillQueries skillQueries) : ControllerBase
{
    
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<SkillDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await skillQueries.GetAll(cancellationToken);

        return entities.Select(SkillDto.FromDomainModel).ToList();
    }

    [HttpGet("[action]/{skillId:guid}")]
    public async Task<ActionResult<SkillDto>> GetById([FromRoute] Guid skillId, CancellationToken cancellationToken)
    {
        var entity = await skillQueries.GetById(new SkillId(skillId), cancellationToken);

        return entity.Match<ActionResult<SkillDto>>(r => SkillDto.FromDomainModel(r), () => NotFound());
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("[action]")]
    public async Task<ActionResult<SkillDto>> Create([FromBody] SkillCreateDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateSkillCommand
        {
            Title = request.Title
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<SkillDto>>(r => SkillDto.FromDomainModel(r), e => e.ToObjectResult());
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("[action]/{skillId:guid}")]
    public async Task<ActionResult<SkillDto>> Update([FromRoute] Guid skillId, [FromBody] SkillUpdateDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateSkillCommand
        {
            SkillId = skillId,
            Title = request.Title
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<SkillDto>>(r => SkillDto.FromDomainModel(r), e => e.ToObjectResult());
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("[action]/{skillId:guid}")] 
    public async Task<ActionResult<SkillDto>> Delete([FromRoute] Guid skillId, CancellationToken cancellationToken)
    {
        var input = new DeleteSkillCommand
        {
            SkillId = skillId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<SkillDto>>(r => SkillDto.FromDomainModel(r), e => e.ToObjectResult());
    }
}