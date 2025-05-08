using FluentValidation;

namespace Application.Skills.Commands;

public class DeleteSkillCommandValidator: AbstractValidator<DeleteSkillCommand>
{
    public DeleteSkillCommandValidator()
    {
        RuleFor(x => x.SkillId).NotEmpty();
    }
}