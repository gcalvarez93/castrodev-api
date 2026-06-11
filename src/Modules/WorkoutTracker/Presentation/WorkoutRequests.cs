// Path: src/Modules/WorkoutTracker/Presentation/WorkoutRequests.cs
namespace Api.Modules.WorkoutTracker.Presentation;

public sealed record CreateRoutineRequest(
    string Name, string Description, string Category,
    List<string> DaysOfWeek, List<ExerciseRequest> Exercises
);

public sealed record UpdateRoutineRequest(
    string Name, string Description, string Category,
    List<string> DaysOfWeek, List<ExerciseRequest> Exercises
);

public sealed record ExerciseRequest(
    string Id, string Name, string MuscleGroup, string Equipment,
    int Sets, int Reps, double WeightKg, int RestSeconds, string? Notes,
    string? ExternalId, bool IsImported
);

public sealed record CompleteSessionRequest(List<SessionExerciseRequest> Exercises);

public sealed record SessionExerciseRequest(
    string ExerciseId, string Name, List<SetLogRequest> Sets, bool IsCompleted
);

public sealed record SetLogRequest(int SetNumber, int Reps, double WeightKg, bool IsCompleted);

public sealed record ImportExternalExerciseRequest(
    string ExternalId, string Name, string MuscleGroup, string Equipment,
    string Difficulty, string Instructions, string? GifUrl,
    int Sets, int Reps, double WeightKg, int RestSeconds
);

public sealed record LogBodyWeightRequest(double WeightKg, DateTime Date, string? Notes);