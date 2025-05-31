using FluentValidation;

namespace Application.Resumes.Commands;

public class DeleteResumeCommandValidator: AbstractValidator<DeleteResumeCommand>
{
    public DeleteResumeCommandValidator() => RuleFor(x => x.ResumeId).NotEmpty();
}