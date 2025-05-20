using FluentValidation;

namespace Application.Users.Commands;

public class UserDeleteCommandValidator: AbstractValidator<UserDeleteCommand>
{
    public UserDeleteCommandValidator() => RuleFor(x => x.UserId).NotEmpty();
}