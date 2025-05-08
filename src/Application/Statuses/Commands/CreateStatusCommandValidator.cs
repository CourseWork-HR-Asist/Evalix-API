using FluentValidation;

namespace Application.Statuses.Commands;

public class CreateStatusCommandValidator: AbstractValidator<CreateStatusCommand>
{
    public CreateStatusCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .MinimumLength(3)
            .NotEmpty();
    }
}