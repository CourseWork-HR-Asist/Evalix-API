using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Evaluations.Exceptions;
using Application.Services.Interfaces;
using Domain.Evaluations;
using Domain.Resumes;
using Domain.Statuses;
using Domain.Vacancies;
using MediatR;

namespace Application.Evaluations.Commands;

public class CreateEvaluationCommand: IRequest<Result<Evaluation, EvaluationExceptions>>
{
    public required Guid VacancyId { get; init; }
    public required Guid ResumeId { get; init; }
    public required Guid StatusId { get; init; }
}

public class CreateEvaluationCommandHandler(
    IEvaluationRepository evaluationRepository,
    IEvaluationQueries evaluationQueries,
    IStatusQueries statusQueries,
    IResumeQueries resumeQueries,
    IVacancyQueries vacancyQueries,
    ILLMService llmService,
    IS3FileService s3FileService) : IRequestHandler<CreateEvaluationCommand, Result<Evaluation, EvaluationExceptions>>
{
    public async Task<Result<Evaluation, EvaluationExceptions>> Handle(CreateEvaluationCommand request,
        CancellationToken cancellationToken)
    {
        var vacancyId = new VacancyId(request.VacancyId);
        var resumeId = new ResumeId(request.ResumeId);
        var statusId = new StatusId(request.StatusId);

        var existingVacancy = await vacancyQueries.GetById(vacancyId, cancellationToken);

        return await existingVacancy.Match<Task<Result<Evaluation, EvaluationExceptions>>>(
            async v =>
            {
                var existingResume = await resumeQueries.GetById(resumeId, cancellationToken);

                return await existingResume.Match<Task<Result<Evaluation, EvaluationExceptions>>>(
                    async r =>
                    {
                        var existingStatus = await statusQueries.GetById(statusId, cancellationToken);

                        return await existingStatus.Match<Task<Result<Evaluation, EvaluationExceptions>>>(
                            async s => await CreateEntity(statusId, v, r, cancellationToken),
                            () => Task.FromResult<Result<Evaluation, EvaluationExceptions>>(
                                new EvaluationStatusNotFoundException(statusId))
                        );
                    },
                    () => Task.FromResult<Result<Evaluation, EvaluationExceptions>>(
                        new EvaluationResumeNotFoundException(resumeId))
                );
            },
            () => Task.FromResult<Result<Evaluation, EvaluationExceptions>>(
                new EvaluationVacancyNotFoundException(vacancyId))
        );
    }

    private async Task<Result<Evaluation, EvaluationExceptions>> CreateEntity(StatusId statusId, Vacancy vacancy,
        Resume resume, CancellationToken cancellationToken)
    {
        try
        {
            var llmString = vacancy.ToLLMString();
            
            var cvFile = await s3FileService.DownloadAsync(resume.FileName, cancellationToken);
            
            var entity = Evaluation.New(EvaluationId.New(), vacancy.Id, resume.Id, statusId, "it is my comment", "it is my score");
            
            return await evaluationRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new EvaluationUnknownException(EvaluationId.Empty(), exception);
        }
    }
}