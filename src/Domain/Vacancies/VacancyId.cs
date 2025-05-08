namespace Domain.Vacancies;

public record VacancyId(Guid Value)
{
    public static VacancyId New() => new(Guid.NewGuid());
    public static VacancyId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}