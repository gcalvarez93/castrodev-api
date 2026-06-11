// Path: src/Modules/WorkoutTracker/Domain/Entities/WorkoutSession.cs
namespace Api.Modules.WorkoutTracker.Domain.Entities;

public sealed class WorkoutSession
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string RoutineId { get; init; } = string.Empty;
    public string RoutineName { get; init; } = string.Empty;
    public List<SessionExercise> Exercises { get; init; } = [];
    public DateTime StartedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public bool IsCompleted { get; init; }
    public int DurationMinutes { get; init; }
    public double TotalVolumeKg { get; init; }
}

public sealed class SessionExercise
{
    public string ExerciseId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public List<SetLog> Sets { get; init; } = [];
    public bool IsCompleted { get; init; }
}

public sealed class SetLog
{
    public int SetNumber { get; init; }
    public int Reps { get; init; }
    public double WeightKg { get; init; }
    public bool IsCompleted { get; init; }
}