// Path: src/Modules/WorkoutTracker/Infrastructure/Repositories/RoutineRepository.cs
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.WorkoutTracker.Infrastructure.Repositories;

public sealed class RoutineRepository(FirestoreDb db) : IRoutineRepository
{
    private CollectionReference Collection => db.Collection("routines_workouttracker");

    public async Task<IEnumerable<Routine>> GetAllAsync(string userId)
    {
        var snapshot = await Collection.WhereEqualTo("userId", userId).OrderBy("name").GetSnapshotAsync();
        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<RoutineDocument>()));
    }

    public async Task<Routine?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;
        var data = doc.ConvertTo<RoutineDocument>();
        if (data.UserId != userId) return null;
        return MapToDomain(doc.Id, data);
    }

    public async Task<string> CreateAsync(Routine routine)
    {
        var doc = Collection.Document();
        await doc.SetAsync(ToDocument(routine));
        return doc.Id;
    }

    public async Task UpdateAsync(Routine routine)
        => await Collection.Document(routine.Id).SetAsync(ToDocument(routine), SetOptions.Overwrite);

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists || doc.ConvertTo<RoutineDocument>().UserId != userId) return;
        await Collection.Document(id).DeleteAsync();
    }

    private static Routine MapToDomain(string id, RoutineDocument doc) => new()
    {
        Id = id, UserId = doc.UserId, Name = doc.Name, Description = doc.Description,
        Category = doc.Category, DaysOfWeek = doc.DaysOfWeek ?? [],
        Exercises = doc.Exercises?.Select(e => new Exercise {
            Id = e.Id, Name = e.Name, MuscleGroup = e.MuscleGroup, Equipment = e.Equipment,
            Sets = e.Sets, Reps = e.Reps, WeightKg = e.WeightKg, RestSeconds = e.RestSeconds,
            Notes = e.Notes, ExternalId = e.ExternalId, IsImported = e.IsImported
        }).ToList() ?? [],
        CreatedAt = doc.CreatedAt
    };

    private static RoutineDocument ToDocument(Routine r) => new()
    {
        UserId = r.UserId, Name = r.Name, Description = r.Description, Category = r.Category,
        DaysOfWeek = r.DaysOfWeek,
        Exercises = r.Exercises.Select(e => new ExerciseDocument {
            Id = e.Id, Name = e.Name, MuscleGroup = e.MuscleGroup, Equipment = e.Equipment,
            Sets = e.Sets, Reps = e.Reps, WeightKg = e.WeightKg, RestSeconds = e.RestSeconds,
            Notes = e.Notes, ExternalId = e.ExternalId, IsImported = e.IsImported
        }).ToList(),
        CreatedAt = r.CreatedAt
    };
}

[FirestoreData] internal sealed class RoutineDocument
{
    [FirestoreProperty("userId")]      public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]        public string Name { get; set; } = string.Empty;
    [FirestoreProperty("description")] public string Description { get; set; } = string.Empty;
    [FirestoreProperty("category")]    public string Category { get; set; } = string.Empty;
    [FirestoreProperty("daysOfWeek")]  public List<string>? DaysOfWeek { get; set; }
    [FirestoreProperty("exercises")]   public List<ExerciseDocument>? Exercises { get; set; }
    [FirestoreProperty("createdAt")]   public DateTime CreatedAt { get; set; }
}

[FirestoreData] internal sealed class ExerciseDocument
{
    [FirestoreProperty("id")]          public string Id { get; set; } = string.Empty;
    [FirestoreProperty("name")]        public string Name { get; set; } = string.Empty;
    [FirestoreProperty("muscleGroup")] public string MuscleGroup { get; set; } = string.Empty;
    [FirestoreProperty("equipment")]   public string Equipment { get; set; } = string.Empty;
    [FirestoreProperty("sets")]        public int Sets { get; set; }
    [FirestoreProperty("reps")]        public int Reps { get; set; }
    [FirestoreProperty("weightKg")]    public double WeightKg { get; set; }
    [FirestoreProperty("restSeconds")] public int RestSeconds { get; set; }
    [FirestoreProperty("notes")]       public string? Notes { get; set; }
    [FirestoreProperty("externalId")]  public string? ExternalId { get; set; }
    [FirestoreProperty("isImported")]  public bool IsImported { get; set; }
}