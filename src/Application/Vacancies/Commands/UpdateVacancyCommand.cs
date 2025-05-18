using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Vacancies.Exceptions;
using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Commands;

public class UpdateVacancyCommand: IRequest<Result<Vacancy, VacancyException>>
{
    public required Guid VacancyId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Experience { get; init; }
    public required string Education { get; init; }
}

public class UpdateVacancyCommandHandler(IVacancyRepository vacancyRepository, IVacancyQueries vacancyQueries)
    : IRequestHandler<UpdateVacancyCommand, Result<Vacancy, VacancyException>>
{
    public async Task<Result<Vacancy, VacancyException>> Handle(UpdateVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancyId = new VacancyId(request.VacancyId);
        var existingVacancy = await vacancyQueries.GetById(vacancyId, cancellationToken);

        return await existingVacancy.Match<Task<Result<Vacancy, VacancyException>>>(
            async v => await UpdateEntity(v, request.Title, request.Description, request.Experience, request.Education,
                cancellationToken),
            () => Task.FromResult<Result<Vacancy, VacancyException>>(new VacancyNotFoundException(vacancyId))
        );
    }
    
    
    private async Task<Result<Vacancy, VacancyException>> UpdateEntity(Vacancy entity, string title, string description, string experience, string education, CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(title, description, experience, education);
            return await vacancyRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new VacancyUnknownException(entity.Id, exception);
        }
    }
}
