using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Skills.Exceptions;
using Domain.Skills;
using MediatR;

namespace Application.Skills.Commands;

public record CreateSkillCommand: IRequest<Result<Skill, SkillExceptions>>
{
    public required string Title { get; init; }
}

public class CreateSkillCommandHandler(ISkillRepository skillRepository,ISkillQueries skillQueries): IRequestHandler<CreateSkillCommand, Result<Skill, SkillExceptions>>
{
    public async Task<Result<Skill, SkillExceptions>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        var existingSkill = await skillQueries.GetByName(request.Title, cancellationToken);
        
        return await existingSkill.Match<Task<Result<Skill, SkillExceptions>>>(
            s => Task.FromResult<Result<Skill, SkillExceptions>>(new SkillAlreadyExistsException(s.Id)),
            async () => await CreateEntity(request.Title, cancellationToken)
        );
    }
    
    private async Task<Result<Skill, SkillExceptions>> CreateEntity(string title, CancellationToken cancellationToken)
    {
        try
        {
            var entity = Skill.New(SkillId.New(), title);
            return await skillRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new SkillUnknownException(SkillId.Empty(), exception);
        }
    }
}