// Path: src/Modules/WorkoutTracker/Domain/Entities/Routine.cs
namespace Api.Modules.WorkoutTracker.Domain.Entities;

public sealed class Routine
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<string> DaysOfWeek { get; init; } = [];
    public List<Exercise> Exercises { get; init; } = [];
    public DateTime CreatedAt { get; init; }
}

public sealed class Exercise
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string MuscleGroup { get; init; } = string.Empty;
    public string Equipment { get; init; } = string.Empty;
    public int Sets { get; init; }
    public int Reps { get; init; }
    public double WeightKg { get; init; }
    public int RestSeconds { get; init; }
    public string? Notes { get; init; }
    public string? ExternalId { get; init; }
    public bool IsImported { get; init; }
}