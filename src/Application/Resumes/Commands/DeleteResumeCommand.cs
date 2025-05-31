using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Resumes.Exceptions;
using Application.Services.Interfaces;
using Domain.Resumes;
using MediatR;

namespace Application.Resumes.Commands;

public class DeleteResumeCommand: IRequest<Result<Resume, ResumeExceptions>>
{
    public required Guid ResumeId { get; init; }
}

public class DeleteResumeCommandHandler(
    IResumeRepository resumeRepository,
    IResumeQueries resumeQueries,
    IS3FileService s3FileService)
    : IRequestHandler<DeleteResumeCommand, Result<Resume, ResumeExceptions>>
{
    public async Task<Result<Resume, ResumeExceptions>> Handle(DeleteResumeCommand request,
        CancellationToken cancellationToken)
    {
        var resumeId = new ResumeId(request.ResumeId);

        var existingResume = await resumeQueries.GetById(resumeId, cancellationToken);

        return await existingResume.Match<Task<Result<Resume, ResumeExceptions>>>(
            async r => await DeleteEntity(r, cancellationToken),
            () => Task.FromResult<Result<Resume, ResumeExceptions>>(new ResumeNotFoundException(resumeId)));
    }

    private async Task<Result<Resume, ResumeExceptions>> DeleteEntity(Resume resume,
        CancellationToken cancellationToken)
    {
        try
        {
            await s3FileService.DeleteAsync(resume.FileName, cancellationToken);
            
            return await resumeRepository.Delete(resume, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ResumeUnknownException(resume.Id, exception);
        }
    }
}