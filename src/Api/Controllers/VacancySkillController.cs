using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.VacancySkills.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("vacancySkills/v1/[controller]")]
[ApiController]
public class VacancySkillController(ISender sender, IVacancySkillQueries vacancySkillQueries) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<VacancySkillDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await vacancySkillQueries.GetAll(cancellationToken);

        return entities.Select(VacancySkillDto.FromDomainModel).ToList();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<VacancySkillDto>> Create(VacancySkillCreateDto createVacancySkillDto,
        CancellationToken cancellationToken)
    {
        var input = new CreateVacancySkillCommand
        {
            VacancyId = createVacancySkillDto.VacancyId,
            SkillId = createVacancySkillDto.SkillId,
            Level = createVacancySkillDto.Level,
            Experience = createVacancySkillDto.Experience
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<VacancySkillDto>>(r => VacancySkillDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [HttpPut("[action]/{id:guid}")]
    public async Task<ActionResult<VacancySkillDto>> Update([FromRoute] Guid id, VacancySkillUpdateDto updateVacancySkillDto,
        CancellationToken cancellationToken)
    {
        var input = new UpdateVacancySkillCommand
        {
            Level = updateVacancySkillDto.Level,
            Experience = updateVacancySkillDto.Experience,
            Id = id
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<VacancySkillDto>>(r => VacancySkillDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [HttpDelete("[action]/{id:guid}")]
    public async Task<ActionResult<VacancySkillDto>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteVacancySkillCommand
        {
            VacancySkillId = id
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<VacancySkillDto>>(r => VacancySkillDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
}