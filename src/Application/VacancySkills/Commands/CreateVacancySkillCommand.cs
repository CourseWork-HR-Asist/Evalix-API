using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Vacancies.Exceptions;
using Application.VacancySkills.Exceptions;
using Domain.Skills;
using Domain.Vacancies;
using Domain.VacancySkills;
using MediatR;

namespace Application.VacancySkills.Commands;

public class CreateVacancySkillCommand: IRequest<Result<VacancySkill, VacancySkillException>>
{
    public required Guid VacancyId { get; init; }
    public required Guid SkillId { get; init; }
    public required int Level { get; init; }
    public required int Experience { get; init; }
}

public class CreateVacancySkillCommandHandler(
    IVacancySkillRepository vacancySkillRepository,
    IVacancySkillQueries vacancySkillQueries,
    IVacancyQueries vacancyQueries,
    ISkillQueries skillQueries)
    : IRequestHandler<CreateVacancySkillCommand, Result<VacancySkill, VacancySkillException>>
{
    public async Task<Result<VacancySkill, VacancySkillException>> Handle(CreateVacancySkillCommand request, CancellationToken cancellationToken)
    {
        var vacancyId = new VacancyId(request.VacancyId);
        var skillId = new SkillId(request.SkillId);
        
        var existingVacancy = await vacancyQueries.GetById(vacancyId, cancellationToken);

        return await existingVacancy.Match<Task<Result<VacancySkill, VacancySkillException>>>(
            async v =>
            {
                var existingSkill = await skillQueries.GetById(skillId, cancellationToken);

                return await existingSkill.Match<Task<Result<VacancySkill, VacancySkillException>>>(
                    async s =>
                    {
                        var existingVacancySkill =
                            await vacancySkillQueries.GetByVacancyIdAndSkillId(vacancyId, skillId, cancellationToken);
                        return await existingVacancySkill.Match<Task<Result<VacancySkill, VacancySkillException>>>(
                            vs => Task.FromResult<Result<VacancySkill, VacancySkillException>>(
                                new VacancySkillAlreadyExistsException(vs.Id)),
                            async () => await CreateEntity(request.VacancyId, request.SkillId, request.Level,
                                request.Experience,
                                cancellationToken)
                        );
                    },
                    () => Task.FromResult<Result<VacancySkill, VacancySkillException>>(
                        new SkillForVacancyNotFoundException(skillId))
                );
            },
            () => Task.FromResult<Result<VacancySkill, VacancySkillException>>(new VacancyForSkillNotFoundException(vacancyId))
        );
    }
    
    private async Task<Result<VacancySkill, VacancySkillException>> CreateEntity(Guid vacancyId, Guid skillId, int level, int experience, CancellationToken cancellationToken)
    {
        try
        {
            var entity = VacancySkill.New(VacancySkillId.New(), new VacancyId(vacancyId), new SkillId(skillId), level, experience);
            return await vacancySkillRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new VacancySkillUnknownException(VacancySkillId.Empty(), exception);
        }
    }
}