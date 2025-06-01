using Domain.Evaluations;
using Domain.Users;

namespace Domain.Resumes;

public class Resume
{
    public ResumeId Id { get; }
    public string FileName { get; private set; }
    public string OriginalFileName { get; private set; }
    public string Url { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public UserId UserId { get; private set; }
    public User? User { get; }
    public ICollection<Evaluation> Evaluations { get; }

    private Resume(ResumeId id, UserId userId, string url, string fileName, string originalFileName)
    {
        Id = id;
        UserId = userId;
        Url = url;
        FileName = fileName;
        CreatedAt = DateTime.UtcNow;
        OriginalFileName = originalFileName;
    }

    public static Resume New(ResumeId id, UserId userId, string url, string fileName, string originalFileName)
        => new(id, userId, url, fileName, originalFileName);
}
