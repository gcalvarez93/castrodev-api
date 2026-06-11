// Path: src/Modules/WorkoutTracker/Application/DTOs/WorkoutSessionDto.cs
namespace Api.Modules.WorkoutTracker.Application.DTOs;

public sealed record WorkoutSessionDto(
    string Id,
    string RoutineId,
    string RoutineName,
    List<SessionExerciseDto> Exercises,
    DateTime StartedAt,
    DateTime? CompletedAt,
    bool IsCompleted,
    int DurationMinutes,
    double TotalVolumeKg
);

public sealed record SessionExerciseDto(
    string ExerciseId,
    string Name,
    List<SetLogDto> Sets,
    bool IsCompleted
);

public sealed record SetLogDto(int SetNumber, int Reps, double WeightKg, bool IsCompleted);