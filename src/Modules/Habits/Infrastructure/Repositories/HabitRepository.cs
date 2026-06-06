// Path: src/Modules/Habits/Infrastructure/Repositories/HabitRepository.cs
using Api.Modules.Habits.Domain.Entities;
using Api.Modules.Habits.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Habits.Infrastructure.Repositories;

public sealed class HabitRepository(FirestoreDb db) : IHabitRepository
{
    private CollectionReference Collection => db.Collection("habits");

    public async Task<Habit?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var habit = doc.ConvertTo<HabitDocument>();
        if (habit.UserId != userId) return null;

        return MapToDomain(doc.Id, habit);
    }

    public async Task<IEnumerable<Habit>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderBy("name")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<HabitDocument>()));
    }

    public async Task<string> CreateAsync(Habit habit)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new HabitDocument
        {
            UserId = habit.UserId,
            Name = habit.Name,
            Icon = habit.Icon,
            Color = habit.Color,
            Frequency = habit.Frequency,
            Streak = habit.Streak,
            CreatedAt = habit.CreatedAt
        });
        return doc.Id;
    }

    public async Task UpdateAsync(Habit habit)
    {
        await Collection.Document(habit.Id).SetAsync(new HabitDocument
        {
            UserId = habit.UserId,
            Name = habit.Name,
            Icon = habit.Icon,
            Color = habit.Color,
            Frequency = habit.Frequency,
            Streak = habit.Streak,
            CreatedAt = habit.CreatedAt
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var habit = doc.ConvertTo<HabitDocument>();
        if (habit.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private static Habit MapToDomain(string id, HabitDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        Name = doc.Name,
        Icon = doc.Icon,
        Color = doc.Color,
        Frequency = doc.Frequency,
        Streak = doc.Streak,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class HabitDocument
{
    [FirestoreProperty("userId")]    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]      public string Name { get; set; } = string.Empty;
    [FirestoreProperty("icon")]      public string Icon { get; set; } = string.Empty;
    [FirestoreProperty("color")]     public string Color { get; set; } = string.Empty;
    [FirestoreProperty("frequency")] public string Frequency { get; set; } = "daily";
    [FirestoreProperty("streak")]    public int Streak { get; set; }
    [FirestoreProperty("createdAt")] public DateTime CreatedAt { get; set; }
}