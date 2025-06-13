using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Resumes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ResumeRepository(ApplicationDbContext context) : IResumeRepository, IResumeQueries
{
    public async Task<Resume> Add(Resume resume, CancellationToken cancellationToken)
    {
        context.Resumes.Add(resume);

        await context.SaveChangesAsync(cancellationToken);

        return resume;
    }

    public async Task<Resume> Update(Resume resume, CancellationToken cancellationToken)
    {
        context.Resumes.Update(resume);

        await context.SaveChangesAsync(cancellationToken);

        return resume;
    }

    public async Task<Resume> Delete(Resume resume, CancellationToken cancellationToken)
    {
        context.Resumes.Remove(resume);

        await context.SaveChangesAsync(cancellationToken);

        return resume;
    }

    public async Task<IReadOnlyList<Resume>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Resumes
            .AsNoTracking()
            .Include(x => x.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Resume>> GetById(ResumeId id, CancellationToken cancellationToken)
    {
        var resume = await context.Resumes
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return resume == null ? Option.None<Resume>() : Option.Some(resume);
    }

    public async Task<IReadOnlyList<Resume>> GetByUserId(UserId userId, CancellationToken cancellationToken)
    {
        return await context.Resumes
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Include(x => x.User)
            .ToListAsync(cancellationToken);
    }
}