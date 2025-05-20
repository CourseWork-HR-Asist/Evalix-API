using FluentValidation;

namespace Application.Skills.Commands;

public class UpdateSkillCommandValidator: AbstractValidator<UpdateSkillCommand>
{
    public UpdateSkillCommandValidator()
    {
        RuleFor(x => x.SkillId).NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(200);
    }
}