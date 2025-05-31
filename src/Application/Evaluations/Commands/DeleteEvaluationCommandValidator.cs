using FluentValidation;

namespace Application.Evaluations.Commands;

public class DeleteEvaluationCommandValidator: AbstractValidator<DeleteEvaluationCommand>
{
    public DeleteEvaluationCommandValidator() => RuleFor(x => x.EvaluationId).NotEmpty();
}