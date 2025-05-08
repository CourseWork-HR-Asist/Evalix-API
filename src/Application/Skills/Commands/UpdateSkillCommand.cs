using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Skills.Exceptions;
using Domain.Skills;
using MediatR;

namespace Application.Skills.Commands;

public record UpdateSkillCommand: IRequest<Result<Skill, SkillExceptions>>
{
    public required Guid SkillId { get; init; }
    public required string Title { get; init; }
}

public class UpdateSkillCommandHandler(ISkillRepository skillRepository, ISkillQueries skillQueries): IRequestHandler<UpdateSkillCommand, Result<Skill, SkillExceptions>>
{
    public async Task<Result<Skill, SkillExceptions>> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        var skillId = new SkillId(request.SkillId);
        var existingSkill = await skillQueries.GetById(skillId, cancellationToken);

        return await existingSkill.Match(
            async s =>
            {
                var existingSkillWithSameName = await skillQueries.GetByName(request.Title, cancellationToken);

                return await existingSkillWithSameName.Match(
                    st => Task.FromResult<Result<Skill, SkillExceptions>>(new SkillAlreadyExistsException(st.Id)),
                    async () => await UpdateEntity(s, request.Title, cancellationToken)
                );
            },
            () => Task.FromResult<Result<Skill, SkillExceptions>>(new SkillNotFoundException(skillId))
        );
    }
    
    private async Task<Result<Skill, SkillExceptions>> UpdateEntity(Skill entity, string title, CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(title);
            return await skillRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new SkillUnknownException(entity.Id, exception);
        }
    }
}

