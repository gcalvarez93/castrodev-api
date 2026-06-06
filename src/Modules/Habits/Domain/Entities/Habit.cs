// Path: src/Modules/Habits/Domain/Entities/Habit.cs
namespace Api.Modules.Habits.Domain.Entities;

public sealed class Habit
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string Frequency { get; init; } = "daily"; // daily | weekly
    public int Streak { get; init; } = 0;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}