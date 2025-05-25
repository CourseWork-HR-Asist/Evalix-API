using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.VacancySkills.Exceptions;
using Domain.VacancySkills;
using MediatR;

namespace Application.VacancySkills.Commands;

public class UpdateVacancySkillCommand: IRequest<Result<VacancySkill, VacancySkillException>>
{
    public required Guid Id { get; init; }
    public required int Level { get; init; }
    public required int Experience { get; init; }
}

public class UpdateVacancySkillCommandHandler(
    IVacancySkillRepository vacancySkillRepository,
    IVacancySkillQueries vacancySkillQueries)
    : IRequestHandler<UpdateVacancySkillCommand, Result<VacancySkill, VacancySkillException>>
{
    public async Task<Result<VacancySkill, VacancySkillException>> Handle(UpdateVacancySkillCommand request, CancellationToken cancellationToken)
    {
        var vacancySkillId = new VacancySkillId(request.Id);
        
        var existingVacancySkill = await vacancySkillQueries.GetById(vacancySkillId, cancellationToken);
        return await existingVacancySkill.Match<Task<Result<VacancySkill, VacancySkillException>>>(
            async r => await UpdateEntity(r, request.Level, request.Experience, cancellationToken),
            () => Task.FromResult<Result<VacancySkill, VacancySkillException>>(new VacancySkillNotFoundException(vacancySkillId))
        );
    }

    private async Task<Result<VacancySkill, VacancySkillException>> UpdateEntity(VacancySkill entity, int level, int experience,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(level, experience);
            return await vacancySkillRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new VacancySkillUnknownException(entity.Id, exception);
        }
    }
}