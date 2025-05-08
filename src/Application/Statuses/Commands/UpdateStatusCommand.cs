using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Statuses.Exceptions;
using Domain.Statuses;
using MediatR;

namespace Application.Statuses.Commands;

public class UpdateStatusCommand: IRequest<Result<Status, StatusExceptions>>
{
    public required Guid StatusId { get; init; }
    public required string Title { get; init; }
}

public class UpdateStatusCommandHandler(IStatusRepository statusRepository, IStatusQueries statusQueries)
    : IRequestHandler<UpdateStatusCommand, Result<Status, StatusExceptions>>
{
    public async Task<Result<Status, StatusExceptions>> Handle(UpdateStatusCommand request,
        CancellationToken cancellationToken)
    {
        var statusId = new StatusId(request.StatusId);

        var existingStatus = await statusQueries.GetById(statusId, cancellationToken);

        return await existingStatus.Match(
            async s =>
            {
                var existingStatusWithSameName = await statusQueries.GetByTitle(request.Title, cancellationToken);

                return await existingStatusWithSameName.Match<Task<Result<Status, StatusExceptions>>>(
                    t => Task.FromResult<Result<Status, StatusExceptions>>(new StatusAlreadyExistsException(t.Id)),
                    async () => await UpdateEntity(s, request.Title, cancellationToken)
                );
            },
            () => Task.FromResult<Result<Status, StatusExceptions>>(new StatusNotFoundException(statusId))
        );
    }

    private async Task<Result<Status, StatusExceptions>> UpdateEntity(Status status, string title,
        CancellationToken cancellationToken)
    {
        try
        {
            status.UpdateDetails(title);
            return await statusRepository.Update(status, cancellationToken);
        }
        catch (Exception exception)
        {
            return new StatusUnknownException(StatusId.Empty(), exception);
        }
    }
}