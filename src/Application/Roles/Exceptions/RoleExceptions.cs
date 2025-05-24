using Domain.Roles;

namespace Application.Roles.Exceptions;

public class RoleExceptions(RoleId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public RoleId RoleId { get; } = id;
}

public class RoleNotFoundException(RoleId id) : RoleExceptions(id, $"Role under id: {id} not found!");

public class RoleWithNameAlreadyExistsException(RoleId id) : RoleExceptions(id, $"Role under such name already exists! {id}");

public class RoleUnknownException(RoleId id, Exception innerException)
    : RoleExceptions(id, $"Unknown exception for the Role under id: {id}!", innerException);