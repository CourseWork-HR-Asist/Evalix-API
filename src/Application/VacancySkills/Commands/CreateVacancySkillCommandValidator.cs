using FluentValidation;

namespace Application.VacancySkills.Commands;

public class CreateVacancySkillCommandValidator: AbstractValidator<CreateVacancySkillCommand>
{
    public CreateVacancySkillCommandValidator()
    {
        RuleFor(x => x.VacancyId).NotEmpty();
        RuleFor(x => x.SkillId).NotEmpty();
        RuleFor(x => x.Experience).NotEmpty().LessThan(50).GreaterThan(-1);
        RuleFor(x => x.Level).NotEmpty().LessThan(50).GreaterThan(-1);
    }
}