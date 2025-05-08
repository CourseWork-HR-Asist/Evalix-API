using Domain.Statuses;

namespace Api.Dtos;

public record StatusCreateDto(string Title)
{
    public static StatusCreateDto FromDomainModel(Status status) => new(status.Title);
}

public record StatusDto(
    Guid? Id,
    string Title)
{
    public static StatusDto FromDomainModel(Status status)
        => new(
            Id: status.Id.Value,
            Title: status.Title);
}

public record StatusUpdateDto(Guid id, string? Title)
{
    public static StatusUpdateDto FromDomainModel(Status status) => new(status.Id.Value, status.Title);
}