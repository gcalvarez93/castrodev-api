// Path: src/Modules/Tasks/Infrastructure/Repositories/BoardRepository.cs
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Tasks.Infrastructure.Repositories;

public sealed class BoardRepository(FirestoreDb db) : IBoardRepository
{
    private CollectionReference Collection => db.Collection("boards");

    public async Task<Board?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var board = doc.ConvertTo<BoardDocument>();
        if (board.UserId != userId) return null;

        return MapToDomain(doc.Id, board);
    }

    public async Task<IEnumerable<Board>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderBy("name")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<BoardDocument>()));
    }

    public async Task<string> CreateAsync(Board board)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new BoardDocument
        {
            UserId = board.UserId,
            Name = board.Name,
            Color = board.Color,
            CreatedAt = board.CreatedAt
        });
        return doc.Id;
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var board = doc.ConvertTo<BoardDocument>();
        if (board.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private static Board MapToDomain(string id, BoardDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        Name = doc.Name,
        Color = doc.Color,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class BoardDocument
{
    [FirestoreProperty("userId")]    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]      public string Name { get; set; } = string.Empty;
    [FirestoreProperty("color")]     public string Color { get; set; } = string.Empty;
    [FirestoreProperty("createdAt")] public DateTime CreatedAt { get; set; }
}