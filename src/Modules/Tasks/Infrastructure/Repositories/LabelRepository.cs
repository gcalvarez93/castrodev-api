// Path: src/Modules/Tasks/Infrastructure/Repositories/LabelRepository.cs
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Tasks.Infrastructure.Repositories;

public sealed class LabelRepository(FirestoreDb db) : ILabelRepository
{
    private CollectionReference Collection => db.Collection("labels_taskmanager");

    public async Task<IEnumerable<Label>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderBy("name")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<LabelDocument>()));
    }

    public async Task<string> CreateAsync(Label label)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new LabelDocument
        {
            UserId = label.UserId,
            Name = label.Name,
            Color = label.Color,
            CreatedAt = label.CreatedAt
        });
        return doc.Id;
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var label = doc.ConvertTo<LabelDocument>();
        if (label.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private static Label MapToDomain(string id, LabelDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        Name = doc.Name,
        Color = doc.Color,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class LabelDocument
{
    [FirestoreProperty("userId")]    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]      public string Name { get; set; } = string.Empty;
    [FirestoreProperty("color")]     public string Color { get; set; } = string.Empty;
    [FirestoreProperty("createdAt")] public DateTime CreatedAt { get; set; }
}