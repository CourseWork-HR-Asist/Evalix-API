using Domain.Evaluations;

namespace Application.Common.Interfaces.Repositories;

public interface IEvaluationRepository
{
    Task<Evaluation> Add (Evaluation evaluation, CancellationToken cancellationToken);
    Task<Evaluation> Update (Evaluation evaluation, CancellationToken cancellationToken);
    Task<Evaluation> Delete (Evaluation evaluation, CancellationToken cancellationToken);
}