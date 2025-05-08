using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Statuses.Commands;
using Domain.Statuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("statuses/v1/[controller]")]
[ApiController]
public class StatusController(ISender sender, IStatusQueries statusQueries) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<StatusDto>>> GetAll(CancellationToken cancellationToken)
    {
        var statuses = await statusQueries.GetAll(cancellationToken);
        
        return statuses.Select(StatusDto.FromDomainModel).ToList();
    }
    
    [HttpGet("[action]/{id:guid}")]
    public async Task<ActionResult<StatusDto>> GetById([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var status = await statusQueries.GetById(new StatusId(id), cancellationToken);
        
        return status.Match<ActionResult<StatusDto>>(s => StatusDto.FromDomainModel(s), () => NotFound());
    }
    
    [HttpPost("[action]")] 
    public async Task<ActionResult<StatusDto>> Create([FromBody] StatusCreateDto request, CancellationToken cancellationToken)
    {
        var input = new CreateStatusCommand
        {
            Title = request.Title
        };
        
        var result = await sender.Send(input, cancellationToken);

        
        return result.Match<ActionResult<StatusDto>>(s => StatusDto.FromDomainModel(s), e => e.ToObjectResult());
    }
    
    [HttpPut("[action]/{id:guid}")] 
    public async Task<ActionResult<StatusDto>> Update([FromRoute] Guid id, [FromBody] StatusUpdateDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateStatusCommand
        {
            StatusId = id,
            Title = request.Title
        };
        
        var result = await sender.Send(input, cancellationToken);

        
        return result.Match<ActionResult<StatusDto>>(s => StatusDto.FromDomainModel(s), e => e.ToObjectResult());
    }

    [HttpDelete("[action]/{id:guid}")]
    public async Task<ActionResult<StatusDto>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteStatusCommand
        {
            StatusId = id
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<StatusDto>>(r => StatusDto.FromDomainModel(r), e => e.ToObjectResult());
    }
}