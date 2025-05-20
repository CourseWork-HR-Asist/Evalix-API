using Domain.Skills;
using Domain.Vacancies;
using Domain.VacancySkills;

namespace Application.VacancySkills.Exceptions;

public class VacancySkillException(VacancySkillId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public VacancySkillId VacancySkillId { get; } = id;
}

public class VacancySkillNotFoundException(VacancySkillId id) : VacancySkillException(id, "Vacancy skill not found") { }

public class VacancySkillAlreadyExistsException(VacancySkillId id) : VacancySkillException(id, "Vacancy skill already exists") { }

public class SkillForVacancyNotFoundException(SkillId id) : VacancySkillException(VacancySkillId.Empty(), $"Skill not found{id}") { }

public class VacancyForSkillNotFoundException(VacancyId id) : VacancySkillException(VacancySkillId.Empty(), $"Vacancy not found{id}") { }

public class VacancySkillUnknownException(VacancySkillId id, Exception innerException) : VacancySkillException(id, "Unknown exception", innerException) { }