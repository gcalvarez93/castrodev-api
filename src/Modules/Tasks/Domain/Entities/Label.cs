// Path: src/Modules/Tasks/Domain/Entities/Label.cs
namespace Api.Modules.Tasks.Domain.Entities;

public sealed class Label
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}