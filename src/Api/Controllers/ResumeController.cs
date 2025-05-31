using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Resumes.Commands;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("resumes/v1/[controller]")]
[ApiController]
public class ResumeController(ISender sender, IResumeQueries roleQueries) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<ResumeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await roleQueries.GetAll(cancellationToken);

        return entities.Select(ResumeDto.FromDomainModel).ToList();
    }
    
    [HttpGet("[action]/{id:guid}")]
    public async Task<ActionResult<IReadOnlyList<ResumeDto>>> GetByUserId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var entities = await roleQueries.GetByUserId(new UserId(id), cancellationToken);
        
        return entities.Select(ResumeDto.FromDomainModel).ToList();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<ResumeDto>> Create([FromForm] IFormFile resumeFile, Guid userId,
        CancellationToken cancellationToken)
    {
        
        await using var stream = resumeFile.OpenReadStream();
        
        var fileName = resumeFile.FileName";
        var contentType = resumeFile.ContentType;
        
        var input = new CreateResumeCommand()
        {
            UserId = userId,
            ResumeFile = stream,
            FileName = fileName,
            ContentType = contentType
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ResumeDto>>(
            r => ResumeDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("[action]/{id:guid}")]
    public async Task<ActionResult<ResumeDto>> Delete([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteResumeCommand()
        {
            ResumeId = id
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ResumeDto>>(r => ResumeDto.FromDomainModel(r), e => e.ToObjectResult());
    }

}
