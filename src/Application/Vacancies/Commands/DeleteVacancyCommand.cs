using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Vacancies.Exceptions;
using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Commands;

public class DeleteVacancyCommand: IRequest<Result<Vacancy, VacancyException>>
{
    public required Guid VacancyId { get; init; }
}

public class DeleteVacancyCommandHandler(IVacancyRepository vacancyRepository, IVacancyQueries vacancyQueries)
    : IRequestHandler<DeleteVacancyCommand, Result<Vacancy, VacancyException>>
{
    public async Task<Result<Vacancy, VacancyException>> Handle(DeleteVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancyId = new VacancyId(request.VacancyId);
        
        var existingVacancy = await vacancyQueries.GetById(vacancyId, cancellationToken);

        return await existingVacancy.Match<Task<Result<Vacancy, VacancyException>>>(
            async v => await DeleteEntity(v, cancellationToken),
            () => Task.FromResult<Result<Vacancy, VacancyException>>(new VacancyNotFoundException(vacancyId))
        );
    }
    
    private async Task<Result<Vacancy, VacancyException>> DeleteEntity(Vacancy entity, CancellationToken cancellationToken)
    {
        try
        {
            return await vacancyRepository.Delete(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new VacancyUnknownException(entity.Id, exception);
        }
    }
}