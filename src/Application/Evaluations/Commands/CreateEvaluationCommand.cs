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

        var existingVacancy = await vacancyQueries.GetById(vacancyId, cancellationToken);

        return await existingVacancy.Match<Task<Result<Evaluation, EvaluationExceptions>>>(
            async v =>
            {
                var existingResume = await resumeQueries.GetById(resumeId, cancellationToken);

                return await existingResume.Match<Task<Result<Evaluation, EvaluationExceptions>>>(
                    async r =>
                    {
                        var existingStatus = await statusQueries.GetByTitle("Analyzed", cancellationToken);

                        return await existingStatus.Match<Task<Result<Evaluation, EvaluationExceptions>>>(
                            async s => await CreateEntity(s.Id, v, r, cancellationToken),
                            () => Task.FromResult<Result<Evaluation, EvaluationExceptions>>(
                                new EvaluationStatusNotFoundException(StatusId.Empty()))
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
            
            var score = await llmService.MatchRequirementsFromPdfAsync(cvFile, resume.FileName, llmString);
            
            var entity = Evaluation.New(EvaluationId.New(), vacancy.Id, resume.Id, statusId, score.SummaryEn, score.MatchScore);
            
            return await evaluationRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return new EvaluationUnknownException(EvaluationId.Empty(), exception);
        }
    }
}