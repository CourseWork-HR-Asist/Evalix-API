using FluentValidation;

namespace Application.Roles.Commands;

public class CreateRoleCommandValidator: AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
    }
}