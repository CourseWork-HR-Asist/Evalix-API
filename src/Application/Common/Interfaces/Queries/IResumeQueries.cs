using Domain.Resumes;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IResumeQueries
{
    Task<IReadOnlyList<Resume>> GetAll (CancellationToken cancellationToken);
    Task<Option<Resume>> GetById(ResumeId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Resume>> GetByUserId(UserId userId, CancellationToken cancellationToken);
}