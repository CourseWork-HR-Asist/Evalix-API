using FluentValidation;

namespace Application.Vacancies.Commands;

public class CreateVacancyCommandValidator: AbstractValidator<CreateVacancyCommand>
{
    public CreateVacancyCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(12).MaximumLength(1000);
        RuleFor(x => x.RequiredEducation).NotEmpty().MinimumLength(3).MaximumLength(1000);
        RuleFor(x => x.RequiredExperience).NotEmpty().MinimumLength(3).MaximumLength(1000);
        RuleFor(x => x.RecruiterId).NotEmpty();
    }
}