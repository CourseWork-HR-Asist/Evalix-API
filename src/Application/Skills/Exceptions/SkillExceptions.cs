using Domain.Skills;

namespace Application.Skills.Exceptions;

public class SkillExceptions(SkillId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public SkillId SkillId { get; } = id;
}

public class SkillNotFoundException(SkillId id) : SkillExceptions(id, "Skill not found");

public class SkillAlreadyExistsException(SkillId id) : SkillExceptions(id, "Skill already exists");

public class SkillUnknownException(SkillId id, Exception innerException)
    : SkillExceptions(id, "Unknown exception", innerException);