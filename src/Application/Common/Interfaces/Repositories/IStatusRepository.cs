using Domain.Statuses;

namespace Application.Common.Interfaces.Repositories;

public interface IStatusRepository
{
    Task<Status> Add (Status status, CancellationToken cancellationToken);
    Task<Status> Update (Status status, CancellationToken cancellationToken);
    Task<Status> Delete (Status status, CancellationToken cancellationToken);
}