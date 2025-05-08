using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Queries;
using Domain.Statuses;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class StatusRepository(ApplicationDbContext context): IStatusRepository, IStatusQueries
{
    public async Task<Status> Add(Status status, CancellationToken cancellationToken)
    {
        context.Statuses.Add(status);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return status;
    }

    public async Task<Status> Update(Status status, CancellationToken cancellationToken)
    {
        context.Statuses.Update(status);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return status;
    }

    public async Task<Status> Delete(Status status, CancellationToken cancellationToken)
    {
        context.Statuses.Remove(status);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return status;
    }

    public async Task<IReadOnlyList<Status>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Statuses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Status>> GetById(StatusId id, CancellationToken cancellationToken)
    {
        var status = await context.Statuses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return status == null ? Option.None<Status>() : Option.Some(status);
    }

    public async Task<Option<Status>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var status = await context.Statuses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return status == null ? Option.None<Status>() : Option.Some(status);
    }
}