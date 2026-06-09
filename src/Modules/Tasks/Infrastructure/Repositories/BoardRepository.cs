// Path: src/Modules/Tasks/Infrastructure/Repositories/BoardRepository.cs
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Tasks.Infrastructure.Repositories;

public sealed class BoardRepository(FirestoreDb db) : IBoardRepository
{
    private CollectionReference Collection => db.Collection("boards_taskmanager");
    private CollectionReference TasksCollection => db.Collection("tasks_taskmanager");

    public async Task<Board?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var board = doc.ConvertTo<BoardDocument>();
        if (board.UserId != userId) return null;

        var taskCount = await GetTaskCountAsync(id, userId);
        return MapToDomain(doc.Id, board, taskCount);
    }

    public async Task<IEnumerable<Board>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderBy("name")
            .GetSnapshotAsync();

        var boards = new List<Board>();
        foreach (var doc in query.Documents)
        {
            var taskCount = await GetTaskCountAsync(doc.Id, userId);
            boards.Add(MapToDomain(doc.Id, doc.ConvertTo<BoardDocument>(), taskCount));
        }
        return boards;
    }

    public async Task<string> CreateAsync(Board board)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new BoardDocument
        {
            UserId = board.UserId,
            Name = board.Name,
            Description = board.Description,
            Color = board.Color,
            CreatedAt = board.CreatedAt
        });
        return doc.Id;
    }

    public async Task UpdateAsync(Board board)
    {
        await Collection.Document(board.Id).SetAsync(new BoardDocument
        {
            UserId = board.UserId,
            Name = board.Name,
            Description = board.Description,
            Color = board.Color,
            CreatedAt = board.CreatedAt
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var board = doc.ConvertTo<BoardDocument>();
        if (board.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private async Task<int> GetTaskCountAsync(string boardId, string userId)
    {
        var query = await TasksCollection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("boardId", boardId)
            .GetSnapshotAsync();
        return query.Count;
    }

    private static Board MapToDomain(string id, BoardDocument doc, int taskCount = 0) => new()
    {
        Id = id,
        UserId = doc.UserId,
        Name = doc.Name,
        Description = doc.Description,
        Color = doc.Color,
        TaskCount = taskCount,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class BoardDocument
{
    [FirestoreProperty("userId")]      public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]        public string Name { get; set; } = string.Empty;
    [FirestoreProperty("description")] public string Description { get; set; } = string.Empty;
    [FirestoreProperty("color")]       public string Color { get; set; } = string.Empty;
    [FirestoreProperty("createdAt")]   public DateTime CreatedAt { get; set; }
}