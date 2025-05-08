using Domain.Statuses;

namespace Application.Statuses.Exceptions;

public class StatusExceptions(StatusId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public StatusId SkillId { get; } = id;
}

public class StatusNotFoundException(StatusId id) : StatusExceptions(id, "Status not found");

public class StatusAlreadyExistsException(StatusId id) : StatusExceptions(id, "Status already exists");

public class StatusUnknownException(StatusId id, Exception innerException)
    : StatusExceptions(id, "Unknown exception", innerException);