using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles.Commands;

public record UpdateRoleCommand: IRequest<Result<Role, RoleExceptions>>
{
    public required Guid RoleId { get; init; }
    public required string Title { get; init; }
}

public class UpdateRoleCommandHandler(IRoleRepository roleRepository, IRoleQueries roleQueries)
    : IRequestHandler<UpdateRoleCommand, Result<Role, RoleExceptions>>
{
    public async Task<Result<Role, RoleExceptions>> Handle(UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);
        var existingRole = await roleQueries.GetById(roleId, cancellationToken);
        return await existingRole.Match(
            async r =>
            {
                var existingRoleWithSameName =
                    await roleQueries.SearchByTitle(request.Title, cancellationToken);

                return await existingRoleWithSameName.Match(
                    rt => Task.FromResult<Result<Role, RoleExceptions>>(
                        new RoleWithNameAlreadyExistsException(rt.Id)),
                    async () => await UpdateEntity(r, request.Title, cancellationToken));
            },
            () => Task.FromResult<Result<Role, RoleExceptions>>(new RoleNotFoundException(roleId))
        );
    }

    private async Task<Result<Role, RoleExceptions>> UpdateEntity(Role entity, string title,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(title);
            return await roleRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new RoleUnknownException(entity.Id, exception);
        }
    }
}