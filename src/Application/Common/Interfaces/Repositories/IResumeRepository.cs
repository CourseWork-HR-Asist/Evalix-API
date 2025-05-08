using Domain.Resumes;

namespace Application.Common.Interfaces.Repositories;

public interface IResumeRepository
{
    Task<Resume> Add (Resume resume, CancellationToken cancellationToken);
    Task<Resume> Update (Resume resume, CancellationToken cancellationToken);
    Task<Resume> Delete (Resume resume, CancellationToken cancellationToken);
}