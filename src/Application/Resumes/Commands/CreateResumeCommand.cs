using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Resumes.Exceptions;
using Application.Services.Interfaces;
using Domain.Resumes;
using Domain.Users;
using MediatR;

namespace Application.Resumes.Commands;

public class CreateResumeCommand: IRequest<Result<Resume, ResumeExceptions>>
{
    public required Guid UserId { get; init; }
    public Stream? ResumeFile { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
}

public class CreateResumeCommandHandler(
    IResumeRepository resumeRepository,
    IResumeQueries resumeQueries,
    IUserQueries userQueries,
    IS3FileService s3FileService)
    : IRequestHandler<CreateResumeCommand, Result<Resume, ResumeExceptions>>
{
    public async Task<Result<Resume, ResumeExceptions>> Handle(CreateResumeCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);

        var existingUser = await userQueries.GetById(userId, cancellationToken);

        return await existingUser.Match<Task<Result<Resume, ResumeExceptions>>>(
            async r => await CreateEntity(userId, request.ResumeFile, request.FileName, request.ContentType, cancellationToken),
            () => Task.FromResult<Result<Resume, ResumeExceptions>>(new UserForResumeNotFoundException(userId))
        );
    }

    private async Task<Result<Resume, ResumeExceptions>> CreateEntity(UserId userId, Stream resumeFile, string fileName,
        String contentType,
        CancellationToken cancellationToken)
    {
        try
        {
            var url = await s3FileService.UploadAsync(resumeFile, fileName, contentType, cancellationToken);
            var entity = Resume.New(ResumeId.New(), userId, url, fileName);
            return await resumeRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ResumeUnknownException(ResumeId.Empty(), exception);
        }
    }
}