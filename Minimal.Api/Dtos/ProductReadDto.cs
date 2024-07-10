namespace Minimal.Api.Dtos;

public record ProductReadDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public decimal Price { get; init; }
}