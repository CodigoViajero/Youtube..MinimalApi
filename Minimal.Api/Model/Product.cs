namespace Minimal.Api.Model;

public record Product
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public decimal Price { get; init; }
    public DateTime CreatedAt { get; init; }
}