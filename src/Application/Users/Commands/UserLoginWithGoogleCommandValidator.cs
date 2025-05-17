using FluentValidation;

namespace Application.Users.Commands;

public class UserLoginWithGoogleCommandValidator: AbstractValidator<UserLoginWithGoogleCommand>
{
    public UserLoginWithGoogleCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}