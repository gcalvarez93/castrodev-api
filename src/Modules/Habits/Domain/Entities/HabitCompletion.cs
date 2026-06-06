// Path: src/Modules/Habits/Domain/Entities/HabitCompletion.cs
namespace Api.Modules.Habits.Domain.Entities;

public sealed class HabitCompletion
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string HabitId { get; init; } = string.Empty;
    public DateTime CompletedAt { get; init; } = DateTime.UtcNow;
}