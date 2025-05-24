using Domain.Evaluations;

namespace Api.Dtos;

public record EvaluationDto(Guid? Id,
    string? Comment,
    string? Score,
    Guid? ResumeId,
    ResumeDto? Resume,
    Guid? VacancyId,
    VacancyDto? Vacancy,
    Guid? StatusId,
    StatusDto? Status)
{
    public static EvaluationDto FromDomainModel(Evaluation evaluation) => new(
        Id: evaluation.Id.Value,
        Comment: evaluation.Comment,
        Score: evaluation.Score,
        ResumeId: evaluation.ResumeId.Value,
        Resume: evaluation.Resume is null ? null : ResumeDto.FromDomainModel(evaluation.Resume),
        VacancyId: evaluation.VacancyId.Value,
        Vacancy: evaluation.Vacancy is null ? null : VacancyDto.FromDomainModel(evaluation.Vacancy),
        StatusId: evaluation.StatusId.Value,
        Status: evaluation.Status is null ? null : StatusDto.FromDomainModel(evaluation.Status)
    );
} 

public record EvaluationCreateDto(
    string? Comment,
    string? Score,
    Guid? ResumeId,
    Guid? VacancyId,
    Guid? StatusId);
    
public record EvaluationUpdateDto(
    string? Comment,
    string? Score,
    Guid? ResumeId,
    Guid? VacancyId,
    Guid? StatusId);