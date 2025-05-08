using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Evaluations;
using Domain.Resumes;
using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class EvaluationRepository(ApplicationDbContext context): IEvaluationRepository, IEvaluationQueries
{
    public async Task<Evaluation> Add(Evaluation evaluation, CancellationToken cancellationToken)
    {
        context.Evaluations.Add(evaluation);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return evaluation;
    }

    public async Task<Evaluation> Update(Evaluation evaluation, CancellationToken cancellationToken)
    {
        context.Evaluations.Update(evaluation);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return evaluation;
    }

    public async Task<Evaluation> Delete(Evaluation evaluation, CancellationToken cancellationToken)
    {
        context.Evaluations.Remove(evaluation);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return evaluation;
    }

    public async Task<IReadOnlyList<Evaluation>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Evaluations
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Evaluation>> GetById(EvaluationId id, CancellationToken cancellationToken)
    {
        var entity = await context.Evaluations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return entity == null ? Option.None<Evaluation>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Evaluation>> GetByVacancyId(VacancyId id, CancellationToken cancellationToken)
    {
        return await context.Evaluations
            .AsNoTracking()
            .Where(x => x.VacancyId == id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Evaluation>> GetByResumeId(ResumeId id, CancellationToken cancellationToken)
    {
        return await context.Evaluations
            .AsNoTracking()
            .Where(x => x.ResumeId == id)
            .ToListAsync(cancellationToken);
    }
}