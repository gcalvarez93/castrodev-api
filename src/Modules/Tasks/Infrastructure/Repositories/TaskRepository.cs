// Path: src/Modules/Tasks/Infrastructure/Repositories/TaskRepository.cs
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Tasks.Infrastructure.Repositories;

public sealed class TaskRepository(FirestoreDb db) : ITaskRepository
{
    private CollectionReference Collection => db.Collection("tasks_taskmanager");

    public async Task<TaskItem?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var task = doc.ConvertTo<TaskDocument>();
        if (task.UserId != userId) return null;

        return MapToDomain(doc.Id, task);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderByDescending("createdAt")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<TaskDocument>()));
    }

    public async Task<IEnumerable<TaskItem>> GetByBoardAsync(string boardId, string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("boardId", boardId)
            .OrderByDescending("createdAt")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<TaskDocument>()));
    }

    public async Task<string> CreateAsync(TaskItem task)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new TaskDocument
        {
            UserId = task.UserId,
            BoardId = task.BoardId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt
        });
        return doc.Id;
    }

    public async Task UpdateAsync(TaskItem task)
    {
        await Collection.Document(task.Id).SetAsync(new TaskDocument
        {
            UserId = task.UserId,
            BoardId = task.BoardId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var task = doc.ConvertTo<TaskDocument>();
        if (task.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private static TaskItem MapToDomain(string id, TaskDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        BoardId = doc.BoardId,
        Title = doc.Title,
        Description = doc.Description,
        Status = doc.Status,
        Priority = doc.Priority,
        DueDate = doc.DueDate,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class TaskDocument
{
    [FirestoreProperty("userId")]      public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("boardId")]     public string BoardId { get; set; } = string.Empty;
    [FirestoreProperty("title")]       public string Title { get; set; } = string.Empty;
    [FirestoreProperty("description")] public string Description { get; set; } = string.Empty;
    [FirestoreProperty("status")]      public string Status { get; set; } = "pending";
    [FirestoreProperty("priority")]    public string Priority { get; set; } = "medium";
    [FirestoreProperty("dueDate")]     public DateTime? DueDate { get; set; }
    [FirestoreProperty("createdAt")]   public DateTime CreatedAt { get; set; }
}