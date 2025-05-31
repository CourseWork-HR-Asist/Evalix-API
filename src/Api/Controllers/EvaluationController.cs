using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Evaluations.Commands;
using Domain.Evaluations;
using Domain.Resumes;
using Domain.Vacancies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("evaluations/v1/[controller]")]
[ApiController]
public class EvaluationController(ISender sender, IEvaluationQueries evaluationQueries) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<EvaluationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await evaluationQueries.GetAll(cancellationToken);

        return entities.Select(EvaluationDto.FromDomainModel).ToList();
    }

    [HttpGet("[action]/{id:guid}")]
    public async Task<ActionResult<EvaluationDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var entity = await evaluationQueries.GetById(new EvaluationId(id), cancellationToken);

        return entity.Match<ActionResult<EvaluationDto>>(e => EvaluationDto.FromDomainModel(e), () => NotFound());
    }

    [HttpGet("[action]/{id:guid}")]
    public async Task<ActionResult<IReadOnlyList<EvaluationDto>>> GetByResumerId([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var entities = await evaluationQueries.GetByResumeId(new ResumeId(id), cancellationToken);

        return entities.Select(EvaluationDto.FromDomainModel).ToList();
    }

    [HttpGet("[action]/{id:guid}")]
    public async Task<ActionResult<IReadOnlyList<EvaluationDto>>> GetByVacancyId([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var entities = await evaluationQueries.GetByVacancyId(new VacancyId(id), cancellationToken);

        return entities.Select(EvaluationDto.FromDomainModel).ToList();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<EvaluationDto>> Create([FromBody] EvaluationCreateDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateEvaluationCommand
        {
            ResumeId = request.ResumeId,
            VacancyId = request.VacancyId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<EvaluationDto>>(r => EvaluationDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [HttpDelete("[action]/{id:guid}")]
    public async Task<ActionResult<EvaluationDto>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteEvaluationCommand
        {
            EvaluationId = id
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<EvaluationDto>>(r => EvaluationDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
}