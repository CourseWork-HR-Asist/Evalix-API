namespace Domain.Evaluations;

public record EvaluationId(Guid Value)
{
    public static EvaluationId New() => new(Guid.NewGuid());
    public static EvaluationId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}