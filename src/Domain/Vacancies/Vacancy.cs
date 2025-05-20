using Domain.Evaluations;
using Domain.Users;
using Domain.VacancySkills;

namespace Domain.Vacancies;

public class Vacancy
{
    public VacancyId Id { get;}
    public string Title { get; private set; }
    public string Description { get; private set; }
    public UserId RecruiterId { get; private set; }
    public User? Recruiter { get; private set; }
    public string Experience { get; private set; }
    public string Education { get; private set; }
    public ICollection<VacancySkill> VacancySkills { get; } = [];
    public ICollection<Evaluation> Evaluations { get; } = [];
    public DateTime CreatedAt { get; private set; }
    
    private Vacancy(VacancyId id, string title, string description, UserId recruiterId, string experience, string education)
    {
        Id = id;
        Title = title;
        Description = description;
        RecruiterId = recruiterId;
        Experience = experience;
        Education = education;
        CreatedAt = DateTime.UtcNow;
    }

    
    public static Vacancy New(VacancyId id, string title, string description, UserId recruiterId, string experience, string education)
        => new(id, title, description, recruiterId, experience, education);
    
    public void UpdateDetails(string title, string description, string requiredExperience, string requiredEducation)
    {
        Title = title;
        Description = description;
        Experience = requiredExperience;
        Education = requiredEducation;
    }
}