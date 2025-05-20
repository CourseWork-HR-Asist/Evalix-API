using FluentValidation;

namespace Application.Vacancies.Commands;

public class UpdateVacancyCommandValidator: AbstractValidator<UpdateVacancyCommand>
{
    public UpdateVacancyCommandValidator()
    {
        RuleFor(v => v.VacancyId).NotEmpty();
        RuleFor(v => v.Title).MaximumLength(255).MinimumLength(3).NotEmpty(); 
        RuleFor(v => v.Description).MaximumLength(1000).MinimumLength(12).NotEmpty();
        RuleFor(v => v.Education).MaximumLength(255).MinimumLength(3).NotEmpty();
        RuleFor(v => v.Experience).MaximumLength(255).MinimumLength(3).NotEmpty();
    }
}