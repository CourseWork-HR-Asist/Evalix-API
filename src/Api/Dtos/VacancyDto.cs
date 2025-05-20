using Application.VacancySkills.Commands;
using Domain.Vacancies;

namespace Api.Dtos;

public record VacancyDto(
    Guid? Id,
    string? Title,
    string? Description,
    string? Experience,
    string? Education,
    Guid? UserId,
    UserDto? User,
    DateTime? CreatedAt,
    List<SkillDto>? Skills)
{
    public static VacancyDto FromDomainModel(Vacancy vacancy) => new(
        Id: vacancy.Id.Value,
        Title: vacancy.Title,
        Description: vacancy.Description,
        Experience: vacancy.Experience,
        Education: vacancy.Education,
        UserId: vacancy.RecruiterId.Value,
        User: vacancy.Recruiter is null ? null : UserDto.FromDomainModel(vacancy.Recruiter),
        CreatedAt: vacancy.CreatedAt,
        Skills: vacancy.VacancySkills.Select(x => SkillDto.FromDomainModel(x.Skill)).ToList());
}

public record VacancyCreateDto(
    string? Title,
    string? Description,
    string? Experience,
    string? Education,
    Guid UserId,
    List<VacancySkillCreateWithVacancyDto> Skills
);

public record VacancyUpdateDto(
    string? Title,
    string? Description,
    string? Experience,
    string? Education)
{
    public static VacancyUpdateDto FromDomainModel(Vacancy vacancy) => new(
        Title: vacancy.Title,
        Description: vacancy.Description,
        Experience: vacancy.Experience,
        Education: vacancy.Education);
}

public record VacancyWithoutSkillsDto(
    Guid? Id,
    string? Title,
    string? Description,
    string? Experience,
    string? Education,
    Guid? UserId,
    UserDto? User,
    DateTime? CreatedAt)
{
    public static VacancyWithoutSkillsDto FromDomainModel(Vacancy vacancy) => new(
        Id: vacancy.Id.Value,
        Title: vacancy.Title,
        Description: vacancy.Description,
        Experience: vacancy.Experience,
        Education: vacancy.Education,
        UserId: vacancy.RecruiterId.Value,
        User: vacancy.Recruiter is null ? null : UserDto.FromDomainModel(vacancy.Recruiter),
        CreatedAt: vacancy.CreatedAt);
}