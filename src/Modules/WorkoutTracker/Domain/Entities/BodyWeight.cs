// Path: src/Modules/WorkoutTracker/Domain/Entities/BodyWeight.cs
namespace Api.Modules.WorkoutTracker.Domain.Entities;

public sealed class BodyWeight
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public double WeightKg { get; init; }
    public DateTime Date { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
}