// Path: src/Modules/Tasks/Domain/Entities/TaskItem.cs
namespace Api.Modules.Tasks.Domain.Entities;

public sealed class TaskItem
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string BoardId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = "todo"; // todo | inProgress | done
    public string Priority { get; init; } = "medium"; // low | medium | high
    public DateTime? DueDate { get; init; }
    public List<string> Labels { get; init; } = new();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}