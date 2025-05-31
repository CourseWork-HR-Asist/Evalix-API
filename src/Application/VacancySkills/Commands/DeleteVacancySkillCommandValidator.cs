using FluentValidation;

namespace Application.VacancySkills.Commands;

public class DeleteVacancySkillCommandValidator : AbstractValidator<DeleteVacancySkillCommand>
{
    public DeleteVacancySkillCommandValidator()
    {
        RuleFor(x => x.VacancySkillId).NotEmpty();
    }
}