using FluentValidation;

namespace Application.Skills.Commands;

public class CreateSkillCommandValidator: AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .MinimumLength(1)
            .NotEmpty();
    }
}