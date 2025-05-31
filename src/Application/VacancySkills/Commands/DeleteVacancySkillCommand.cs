using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.VacancySkills.Exceptions;
using Domain.VacancySkills;
using MediatR;

namespace Application.VacancySkills.Commands;

public class DeleteVacancySkillCommand: IRequest<Result<VacancySkill, VacancySkillException>>
{
    public required Guid VacancySkillId { get; init; }
}

public class DeleteVacancySkillCommandHandler(
    IVacancySkillRepository vacancySkillRepository,
    IVacancySkillQueries vacancySkillQueries)
    : IRequestHandler<DeleteVacancySkillCommand, Result<VacancySkill, VacancySkillException>>
{
    public async Task<Result<VacancySkill, VacancySkillException>> Handle(DeleteVacancySkillCommand request, CancellationToken cancellationToken)
    {
        var vacancySkillId = new VacancySkillId(request.VacancySkillId);
        
        var existingVacancySkill = await vacancySkillQueries.GetById(vacancySkillId, cancellationToken);
        return await existingVacancySkill.Match<Task<Result<VacancySkill, VacancySkillException>>>(
            async r => await DeleteEntity(r, cancellationToken),
            () => Task.FromResult<Result<VacancySkill, VacancySkillException>>(new VacancySkillNotFoundException(vacancySkillId)));
    }
    
    private async Task<Result<VacancySkill, VacancySkillException>> DeleteEntity(VacancySkill entity, CancellationToken cancellationToken)
    {
        try
        {
            return await vacancySkillRepository.Delete(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new VacancySkillUnknownException(entity.Id, exception);
        }
    }
}