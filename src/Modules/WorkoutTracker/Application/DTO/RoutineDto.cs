// Path: src/Modules/WorkoutTracker/Application/DTOs/RoutineDto.cs
namespace Api.Modules.WorkoutTracker.Application.DTOs;

public sealed record RoutineDto(
    string Id,
    string Name,
    string Description,
    string Category,
    List<string> DaysOfWeek,
    List<ExerciseDto> Exercises,
    DateTime CreatedAt
);

public sealed record ExerciseDto(
    string Id,
    string Name,
    string MuscleGroup,
    string Equipment,
    int Sets,
    int Reps,
    double WeightKg,
    int RestSeconds,
    string? Notes,
    string? ExternalId,
    bool IsImported
);