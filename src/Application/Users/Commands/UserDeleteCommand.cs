using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class UserDeleteCommand: IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
}

public class UserDeleteCommandHandler(IUserQueries userQueries, IUserRepository userRepository) : IRequestHandler<UserDeleteCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        
        var existingUser = await userQueries.GetById(userId, cancellationToken);
        return await existingUser.Match<Task<Result<User, UserException>>>(
            async r => await DeleteEntity(r, cancellationToken),
            () => Task.FromResult<Result<User, UserException>>(new UserNotFoundException(userId)));
    }
    
    
    private async Task<Result<User, UserException>> DeleteEntity(User entity, CancellationToken cancellationToken)
    {
        try
        {
            return await userRepository.Delete(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(entity.Id, exception);
        }
    }
}