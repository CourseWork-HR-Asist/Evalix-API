using FluentValidation;

namespace Application.Statuses.Commands;

public class UpdateStatusCommandValidator: AbstractValidator<UpdateStatusCommand>
{
    public UpdateStatusCommandValidator()
    {
        RuleFor(x => x.StatusId).NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);
    }
}