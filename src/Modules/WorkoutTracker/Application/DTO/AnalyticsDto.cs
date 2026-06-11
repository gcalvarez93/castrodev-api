// Path: src/Modules/WorkoutTracker/Application/DTOs/AnalyticsDto.cs
namespace Api.Modules.WorkoutTracker.Application.DTOs;

public sealed record WeeklySummaryDto(
    int Year,
    int Week,
    int DaysTrained,
    int TotalExercisesCompleted,
    double TotalVolumeKg,
    int TotalDurationMinutes,
    int CurrentStreak,
    List<SessionSummaryDto> Sessions
);

public sealed record SessionSummaryDto(
    string SessionId,
    string RoutineName,
    DateTime Date,
    int DurationMinutes,
    double VolumeKg
);

public sealed record ExerciseProgressDto(
    string ExerciseName,
    List<ProgressPointDto> Points
);

public sealed record ProgressPointDto(DateTime Date, double MaxWeightKg, int TotalReps);

public sealed record ExternalExerciseDto(
    string ExternalId,
    string Name,
    string MuscleGroup,
    string Equipment,
    string Difficulty,
    string Instructions,
    string? GifUrl
);