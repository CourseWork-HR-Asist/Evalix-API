using Domain.Skills;
using Domain.Users;
using Domain.Vacancies;

namespace Application.Vacancies.Exceptions;

public class VacancyException(VacancyId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public VacancyId VacancyId { get; } = id;
}

public class VacancyNotFoundException(VacancyId id) : VacancyException(id, $"Vacancy under id: {id} not found");

public class VacancyAlreadyExistsException(VacancyId id) : VacancyException(id, $"Vacancy under id: {id} already exists");

public class UserNotFoundException(UserId id) : VacancyException(VacancyId.Empty(), $"User under id: {id} not found");

public class SkillNotFoundException(SkillId id) : VacancyException(VacancyId.Empty(), $"Skill under id: {id} not found");

public class VacancyUnknownException(VacancyId id, Exception innerException)
    : VacancyException(id, $"Unknown exception for the Vacancy under id: {id}!", innerException);
