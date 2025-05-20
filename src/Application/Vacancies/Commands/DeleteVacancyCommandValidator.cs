using FluentValidation;

namespace Application.Vacancies.Commands;

public class DeleteVacancyCommandValidator : AbstractValidator<DeleteVacancyCommand>
{
    public DeleteVacancyCommandValidator()
    {
        RuleFor(v => v.VacancyId).NotEmpty();
    }
}