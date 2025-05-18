using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Vacancies.Exceptions;
using Domain.Skills;
using Domain.Users;
using Domain.Vacancies;
using Domain.VacancySkills;
using MediatR;
using Optional;

namespace Application.Vacancies.Commands;

public class CreateVacancyCommand: IRequest<Result<Vacancy, VacancyException>>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string RequiredExperience { get; init; }
    public required string RequiredEducation { get; init; }
    public required Guid RecruiterId { get; init; }
    public required List<VacancySkill> Skills { get; init; }
}

public class CreateVacancyCommandHandler(
    IVacancyRepository vacancyRepository,
    IVacancyQueries vacancyQueries,
    IUserQueries userQueries,
    IVacancySkillRepository vacancySkillRepository)
    : IRequestHandler<CreateVacancyCommand, Result<Vacancy, VacancyException>>
{
    public async Task<Result<Vacancy, VacancyException>> Handle(CreateVacancyCommand request,
        CancellationToken cancellationToken)
    {
        var UserId = new UserId(request.RecruiterId);
        
        var VacancyId = new VacancyId(Guid.NewGuid());
        
        var existingUser = await userQueries.GetById(UserId, cancellationToken);

        return await existingUser.Match<Task<Result<Vacancy, VacancyException>>>(
            async u  =>
            {
                var result = await CreateEntity(VacancyId ,request.Title, request.Description, request.RequiredExperience,
                    request.RequiredEducation, request.RecruiterId, cancellationToken);
                
                foreach (var skill in request.Skills)
                {
                    await CreateEntity(VacancyId, skill.SkillId, skill.Level, skill.Experience, cancellationToken);
                }
                
                return result; 
                
            },
            () => Task.FromResult<Result<Vacancy, VacancyException>>(new UserNotFoundException(UserId))
        );
    }


    private async Task<Result<Vacancy, VacancyException>> CreateEntity(VacancyId id ,string title, string description, string requiredExperience, string requiredEducation, Guid recruiterId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Vacancy.New(id, title, description, new UserId(recruiterId), requiredExperience, requiredEducation);
            
            return await vacancyRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new VacancyUnknownException(VacancyId.Empty(), exception);
        }
    }
    
    private async Task CreateEntity(VacancyId vacancyId, SkillId skillId, int level, int experience, CancellationToken cancellationToken)
    {
        try
        {
            var entity = VacancySkill.New(VacancySkillId.New(),vacancyId, skillId, level, experience);
            await vacancySkillRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            throw new VacancyUnknownException(VacancyId.Empty(), exception);
        }
    }

}
        