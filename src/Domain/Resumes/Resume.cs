using Domain.Evaluations;

namespace Domain.Resumes;

public class Resume
{
    public ResumeId Id { get; }
    public string Url { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<Evaluation> Evaluations { get; }

    private Resume(ResumeId id, string url, DateTime createdAt)
    {
        Id = id;
        Url = url;
        CreatedAt = createdAt;
    }

    public static Resume Create(ResumeId id, string url, DateTime createdAt)
        => new(id, url, createdAt);
}
