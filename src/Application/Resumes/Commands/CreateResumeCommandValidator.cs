using FluentValidation;

namespace Application.Resumes.Commands;

public class CreateResumeCommandValidator: AbstractValidator<CreateResumeCommand>
{
    public CreateResumeCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ResumeFile).NotEmpty();
    }
}