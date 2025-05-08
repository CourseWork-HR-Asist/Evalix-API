namespace Domain.Resumes;

public record ResumeId(Guid Value)
{
    public static ResumeId New() => new(Guid.NewGuid());
    public static ResumeId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}