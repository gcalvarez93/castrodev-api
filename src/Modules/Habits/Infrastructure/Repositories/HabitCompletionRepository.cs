// Path: src/Modules/Habits/Infrastructure/Repositories/HabitCompletionRepository.cs
using Api.Modules.Habits.Domain.Entities;
using Api.Modules.Habits.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Habits.Infrastructure.Repositories;

public sealed class HabitCompletionRepository(FirestoreDb db) : IHabitCompletionRepository
{
    private CollectionReference Collection => db.Collection("completions_habitstracker");

    public async Task<IEnumerable<HabitCompletion>> GetByHabitIdAsync(string habitId, string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("habitId", habitId)
            .OrderByDescending("completedAt")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<HabitCompletionDocument>()));
    }

    public async Task<IEnumerable<HabitCompletion>> GetByDateAsync(string userId, DateTime date)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        var query = await Collection
            .WhereEqualTo("userId", userId)
            .WhereGreaterThanOrEqualTo("completedAt", start)
            .WhereLessThan("completedAt", end)
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<HabitCompletionDocument>()));
    }

    public async Task<string> CreateAsync(HabitCompletion completion)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new HabitCompletionDocument
        {
            UserId = completion.UserId,
            HabitId = completion.HabitId,
            CompletedAt = completion.CompletedAt
        });
        return doc.Id;
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var completion = doc.ConvertTo<HabitCompletionDocument>();
        if (completion.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private static HabitCompletion MapToDomain(string id, HabitCompletionDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        HabitId = doc.HabitId,
        CompletedAt = doc.CompletedAt
    };
}

[FirestoreData]
internal sealed class HabitCompletionDocument
{
    [FirestoreProperty("userId")]      public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("habitId")]     public string HabitId { get; set; } = string.Empty;
    [FirestoreProperty("completedAt")] public DateTime CompletedAt { get; set; }
}