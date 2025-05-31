using Domain.Resumes;
using Domain.Statuses;
using Domain.Vacancies;

namespace Domain.Evaluations;

public class Evaluation
{
    public EvaluationId Id { get; }
    public VacancyId VacancyId { get; private set; }
    public Vacancy? Vacancy { get; private set; }
    public ResumeId ResumeId { get; private set; }
    public Resume? Resume { get; private set; }
    public StatusId StatusId { get; private set; }
    public Status? Status { get; private set; }
    public string Comment { get; private set; }
    public string Score { get; private set; }

    private Evaluation(EvaluationId id, VacancyId vacancyId, ResumeId resumeId, StatusId statusId, string comment, string score)
    {
        Id = id;
        VacancyId = vacancyId;
        ResumeId = resumeId;
        StatusId = statusId;
        Comment = comment;
        Score = score;
    }
    
    public static Evaluation New(EvaluationId id, VacancyId vacancyId, ResumeId resumeId, StatusId statusId, string comment, string score)
        => new Evaluation(id, vacancyId, resumeId, statusId, comment, score);
    
    public void Update(string comment, string score)
    {
        Comment = comment;
        Score = score;
    }
    
    public void UpdateStatus(StatusId statusId)
    {
        StatusId = statusId;
    }
}