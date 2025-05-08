using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Skills.Exceptions;
using Domain.Skills;
using MediatR;

namespace Application.Skills.Commands;

public record DeleteSkillCommand: IRequest<Result<Skill, SkillExceptions>>
{
    public required Guid SkillId { get; init; }
}

public class DeleteSkillCommandHandler(ISkillRepository skillRepository, ISkillQueries skillQueries) : IRequestHandler<DeleteSkillCommand, Result<Skill, SkillExceptions>>
{
    public async Task<Result<Skill, SkillExceptions>> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        var skillId = new SkillId(request.SkillId);
        
        var existingSkill = await skillQueries.GetById(skillId, cancellationToken);
        return await existingSkill.Match<Task<Result<Skill, SkillExceptions>>>(
            async r => await DeleteEntity(r, cancellationToken),
            () => Task.FromResult<Result<Skill, SkillExceptions>>(new SkillNotFoundException(skillId)))
        ;
    }
    
    private async Task<Result<Skill, SkillExceptions>> DeleteEntity(Skill skill, CancellationToken cancellationToken)
    {
        try
        {
            return await skillRepository.Delete(skill, cancellationToken);
        }
        catch (Exception exception)
        {
            return new SkillUnknownException(skill.Id, exception);
        }
    }
}