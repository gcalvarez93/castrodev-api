// Path: src/Modules/WorkoutTracker/Infrastructure/Repositories/WorkoutSessionRepository.cs
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.WorkoutTracker.Infrastructure.Repositories;

public sealed class WorkoutSessionRepository(FirestoreDb db) : IWorkoutSessionRepository
{
    private CollectionReference Collection => db.Collection("sessions_workouttracker");

    public async Task<IEnumerable<WorkoutSession>> GetAllAsync(string userId, string? routineId = null)
    {
        Query query = Collection.WhereEqualTo("userId", userId);
        if (!string.IsNullOrWhiteSpace(routineId))
            query = query.WhereEqualTo("routineId", routineId);
        var snapshot = await query.OrderByDescending("startedAt").GetSnapshotAsync();
        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<SessionDocument>()));
    }

    public async Task<WorkoutSession?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;
        var data = doc.ConvertTo<SessionDocument>();
        if (data.UserId != userId) return null;
        return MapToDomain(doc.Id, data);
    }

    public async Task<string> CreateAsync(WorkoutSession session)
    {
        var doc = Collection.Document();
        await doc.SetAsync(ToDocument(session));
        return doc.Id;
    }

    public async Task UpdateAsync(WorkoutSession session)
        => await Collection.Document(session.Id).SetAsync(ToDocument(session), SetOptions.Overwrite);

    public async Task<IEnumerable<WorkoutSession>> GetByDateRangeAsync(string userId, DateTime from, DateTime to)
    {
        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .WhereGreaterThanOrEqualTo("startedAt", from)
            .WhereLessThanOrEqualTo("startedAt", to)
            .GetSnapshotAsync();
        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<SessionDocument>()));
    }

    private static WorkoutSession MapToDomain(string id, SessionDocument doc) => new()
    {
        Id = id, UserId = doc.UserId, RoutineId = doc.RoutineId, RoutineName = doc.RoutineName,
        Exercises = doc.Exercises?.Select(e => new SessionExercise {
            ExerciseId = e.ExerciseId, Name = e.Name, IsCompleted = e.IsCompleted,
            Sets = e.Sets?.Select(s => new SetLog { SetNumber = s.SetNumber, Reps = s.Reps, WeightKg = s.WeightKg, IsCompleted = s.IsCompleted }).ToList() ?? []
        }).ToList() ?? [],
        StartedAt = doc.StartedAt, CompletedAt = doc.CompletedAt,
        IsCompleted = doc.IsCompleted, DurationMinutes = doc.DurationMinutes, TotalVolumeKg = doc.TotalVolumeKg
    };

    private static SessionDocument ToDocument(WorkoutSession s) => new()
    {
        UserId = s.UserId, RoutineId = s.RoutineId, RoutineName = s.RoutineName,
        Exercises = s.Exercises.Select(e => new SessionExerciseDocument {
            ExerciseId = e.ExerciseId, Name = e.Name, IsCompleted = e.IsCompleted,
            Sets = e.Sets.Select(set => new SetLogDocument { SetNumber = set.SetNumber, Reps = set.Reps, WeightKg = set.WeightKg, IsCompleted = set.IsCompleted }).ToList()
        }).ToList(),
        StartedAt = s.StartedAt, CompletedAt = s.CompletedAt,
        IsCompleted = s.IsCompleted, DurationMinutes = s.DurationMinutes, TotalVolumeKg = s.TotalVolumeKg
    };
}

[FirestoreData] internal sealed class SessionDocument
{
    [FirestoreProperty("userId")]          public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("routineId")]       public string RoutineId { get; set; } = string.Empty;
    [FirestoreProperty("routineName")]     public string RoutineName { get; set; } = string.Empty;
    [FirestoreProperty("exercises")]       public List<SessionExerciseDocument>? Exercises { get; set; }
    [FirestoreProperty("startedAt")]       public DateTime StartedAt { get; set; }
    [FirestoreProperty("completedAt")]     public DateTime? CompletedAt { get; set; }
    [FirestoreProperty("isCompleted")]     public bool IsCompleted { get; set; }
    [FirestoreProperty("durationMinutes")] public int DurationMinutes { get; set; }
    [FirestoreProperty("totalVolumeKg")]   public double TotalVolumeKg { get; set; }
}

[FirestoreData] internal sealed class SessionExerciseDocument
{
    [FirestoreProperty("exerciseId")]  public string ExerciseId { get; set; } = string.Empty;
    [FirestoreProperty("name")]        public string Name { get; set; } = string.Empty;
    [FirestoreProperty("isCompleted")] public bool IsCompleted { get; set; }
    [FirestoreProperty("sets")]        public List<SetLogDocument>? Sets { get; set; }
}

[FirestoreData] internal sealed class SetLogDocument
{
    [FirestoreProperty("setNumber")]   public int SetNumber { get; set; }
    [FirestoreProperty("reps")]        public int Reps { get; set; }
    [FirestoreProperty("weightKg")]    public double WeightKg { get; set; }
    [FirestoreProperty("isCompleted")] public bool IsCompleted { get; set; }
}