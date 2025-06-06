﻿using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Statuses.Exceptions;
using Domain.Statuses;
using MediatR;

namespace Application.Statuses.Commands;

public class CreateStatusCommand: IRequest<Result<Status, StatusExceptions>>
{
    public required string Title { get; init; }
}

public class CreateStatusCommandHandler(IStatusRepository statusRepository, IStatusQueries statusQueries): IRequestHandler<CreateStatusCommand, Result<Status, StatusExceptions>>
{
    public async Task<Result<Status, StatusExceptions>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
    {
        var existingStatus = await statusQueries.GetByTitle(request.Title, cancellationToken);
        
        return await existingStatus.Match<Task<Result<Status, StatusExceptions>>>(
            s => Task.FromResult<Result<Status, StatusExceptions>>(new StatusAlreadyExistsException(s.Id)),
            async () => await CreateEntity(request.Title, cancellationToken)
        );
    }

    private async Task<Result<Status, StatusExceptions>> CreateEntity(string title, CancellationToken cancellationToken)
    {
        try
        {
            var entity = Status.New(StatusId.New(), title);
            return await statusRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new StatusUnknownException(StatusId.Empty(), exception);
        }
    }
}