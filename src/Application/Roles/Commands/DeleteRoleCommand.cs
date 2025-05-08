using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles.Commands;

public record DeleteRoleCommand: IRequest<Result<Role, RoleExceptions>>
{
    public required Guid RoleId { get; init; }
}

public class DeleteRoleCommandHandler(IRoleRepository roleRepository, IRoleQueries roleQueries) : IRequestHandler<DeleteRoleCommand, Result<Role, RoleExceptions>>
{
    public async Task<Result<Role, RoleExceptions>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);
        var existingRole = await roleQueries.GetById(roleId, cancellationToken);
        return await existingRole.Match<Task<Result<Role, RoleExceptions>>>(
            async r => await DeleteEntity(r, cancellationToken),
            () => Task.FromResult<Result<Role, RoleExceptions>>(new RoleNotFoundException(roleId)));
    }
    
    private async Task<Result<Role, RoleExceptions>> DeleteEntity(Role entity, CancellationToken cancellationToken)
    {
        try
        {
            return await roleRepository.Delete(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new RoleUnknownException(entity.Id, exception);
        }
    }
}