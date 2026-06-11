// Path: src/Modules/WorkoutTracker/Infrastructure/Repositories/BodyWeightRepository.cs
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.WorkoutTracker.Infrastructure.Repositories;

public sealed class BodyWeightRepository(FirestoreDb db) : IBodyWeightRepository
{
    private CollectionReference Collection => db.Collection("bodyweight_workouttracker");

    public async Task<IEnumerable<BodyWeight>> GetAllAsync(string userId)
    {
        var snapshot = await Collection.WhereEqualTo("userId", userId).OrderByDescending("date").GetSnapshotAsync();
        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<BodyWeightDocument>()));
    }

    public async Task<string> CreateAsync(BodyWeight entry)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new BodyWeightDocument {
            UserId = entry.UserId, WeightKg = entry.WeightKg,
            Date = entry.Date, Notes = entry.Notes, CreatedAt = entry.CreatedAt
        });
        return doc.Id;
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists || doc.ConvertTo<BodyWeightDocument>().UserId != userId) return;
        await Collection.Document(id).DeleteAsync();
    }

    private static BodyWeight MapToDomain(string id, BodyWeightDocument doc) => new()
    {
        Id = id, UserId = doc.UserId, WeightKg = doc.WeightKg,
        Date = doc.Date, Notes = doc.Notes, CreatedAt = doc.CreatedAt
    };
}

[FirestoreData] internal sealed class BodyWeightDocument
{
    [FirestoreProperty("userId")]    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("weightKg")]  public double WeightKg { get; set; }
    [FirestoreProperty("date")]      public DateTime Date { get; set; }
    [FirestoreProperty("notes")]     public string? Notes { get; set; }
    [FirestoreProperty("createdAt")] public DateTime CreatedAt { get; set; }
}