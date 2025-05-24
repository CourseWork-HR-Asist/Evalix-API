using Domain.Resumes;
using Domain.Users;

namespace Application.Resumes.Exceptions;

public class ResumeExceptions(ResumeId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ResumeId ResumeId { get; } = id;
}

public class ResumeNotFoundException(ResumeId id) : ResumeExceptions(id, "Resume not found");

public class ResumeAlreadyExistsException(ResumeId id) : ResumeExceptions(id, "Resume already exists");

public class UserForResumeNotFoundException(UserId id) : ResumeExceptions(ResumeId.Empty(), $"User {id} not found for the resume");

public class ResumeUnknownException(ResumeId id, Exception innerException) : ResumeExceptions(id, "Unknown exception", innerException);