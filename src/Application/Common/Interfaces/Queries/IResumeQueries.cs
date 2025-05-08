using Domain.Resumes;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IResumeQueries
{
    Task<IReadOnlyList<Resume>> GetAll (CancellationToken cancellationToken);
    Task<Option<Resume>> GetById(ResumeId id, CancellationToken cancellationToken);
}