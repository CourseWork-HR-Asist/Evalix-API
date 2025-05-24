using Domain.Resumes;

namespace Api.Dtos;

public record ResumeDto(
    Guid? Id,
    string? Url,
    DateTime? CreatedAt,
    Guid? UserId,
    UserDto? User
    )
{
    public static ResumeDto FromDomainModel(Resume resume) => new(
        Id: resume.Id.Value,
        Url: resume.Url,
        CreatedAt: resume.CreatedAt,
        UserId: resume.UserId.Value,
        User: resume.User is null ? null : UserDto.FromDomainModel(resume.User));
}

public record ResumeCreateDto(
    string? Url,
    Guid? UserId);
    
    

public record ResumeUpdateDto(
    string? Url);