// Path: src/Modules/Finance/Domain/Category.cs
namespace Api.Modules.Finance.Domain.Entities;

public sealed class Category
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}