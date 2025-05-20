using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Vacancies.Commands;
using Domain.Skills;
using Domain.Vacancies;
using Domain.VacancySkills;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional.Linq;

namespace Api.Controllers;

[Route("users/v1/[controller]")]
[ApiController]
public class VacancyController(ISender sender, IVacancyQueries vacancyQueries) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<VacancyDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await vacancyQueries.GetAll(cancellationToken);

        return entities.Select(VacancyDto.FromDomainModel).ToList();
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<VacancyDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var entity = await vacancyQueries.GetById(new VacancyId(id), cancellationToken);

        return entity.Match<ActionResult<VacancyDto>>(v => VacancyDto.FromDomainModel(v), () => NotFound());
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<VacancyWithoutSkillsDto>> Create([FromBody] VacancyCreateDto request,
        CancellationToken cancellationToken)
    {

        var skills = request.Skills
            .Select(s => s.ToDomain())
            .ToList();
        
        var input = new CreateVacancyCommand
        {
            Title = request.Title,
            Description = request.Description,
            RequiredExperience = request.Experience,
            RequiredEducation = request.Education,
            RecruiterId = request.UserId,
            Skills = skills
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<VacancyWithoutSkillsDto>>(v => VacancyWithoutSkillsDto.FromDomainModel(v), e => e.ToObjectResult());
    }
    
    [HttpDelete("[action]")]
    public async Task<ActionResult<VacancyDto>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteVacancyCommand
        {
            VacancyId = id
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<VacancyDto>>(r => VacancyDto.FromDomainModel(r), e => e.ToObjectResult());
    }
    
    [HttpPut("[action]")]
    public async Task<ActionResult<VacancyDto>> Update(Guid vacancyId, [FromBody] VacancyUpdateDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateVacancyCommand
        {
            VacancyId = vacancyId,
            Title = request.Title,
            Description = request.Description,
            Experience = request.Experience,
            Education = request.Education
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<VacancyDto>>(r => VacancyDto.FromDomainModel(r), e => e.ToObjectResult());
    }
}

