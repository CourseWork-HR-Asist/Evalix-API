using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Evaluations.Exceptions;
using Domain.Evaluations;
using MediatR;

namespace Application.Evaluations.Commands;

public class DeleteEvaluationCommand: IRequest<Result<Evaluation, EvaluationExceptions>>
{
    public required Guid EvaluationId { get; init; }
}

public class DeleteEvaluationCommandHandler(
    IEvaluationRepository evaluationRepository,
    IEvaluationQueries evaluationQueries)
    : IRequestHandler<DeleteEvaluationCommand, Result<Evaluation, EvaluationExceptions>>
{
    public async Task<Result<Evaluation, EvaluationExceptions>> Handle(DeleteEvaluationCommand request, CancellationToken cancellationToken)
    {
        var evaluationId = new EvaluationId(request.EvaluationId);
        
        var entity = await evaluationQueries.GetById(evaluationId, cancellationToken);
        
        return await entity.Match<Task<Result<Evaluation, EvaluationExceptions>>>(
            e => DeleteEntity(e, cancellationToken),
            () => Task.FromResult<Result<Evaluation, EvaluationExceptions>>(new EvaluationNotFoundException(evaluationId))
        );
    }

    private async Task<Result<Evaluation, EvaluationExceptions>> DeleteEntity(Evaluation entity,
        CancellationToken cancellationToken)
    {
        try
        {
            return await evaluationRepository.Delete(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new EvaluationUnknownException(entity.Id, exception);
        }
    }
}