using FluentValidation;

namespace Application.Evaluations.Commands;

public class CreateEvaluationCommandValidator: AbstractValidator<CreateEvaluationCommand>
{
    public CreateEvaluationCommandValidator()
    {
        RuleFor(x => x.VacancyId).NotEmpty();
        RuleFor(x => x.ResumeId).NotEmpty();

    }
}