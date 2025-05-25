using FluentValidation;

namespace Application.VacancySkills.Commands;

public class UpdateVacancySkillCommandValidator: AbstractValidator<UpdateVacancySkillCommand>
{
    public UpdateVacancySkillCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Experience).NotEmpty().LessThan(50).GreaterThan(0);
        RuleFor(x => x.Level).NotEmpty().LessThan(5).GreaterThan(-1);
    } 
}