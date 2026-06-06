// Path: src/Modules/Tasks/Domain/Repositories/ITaskRepository.cs
using Api.Modules.Tasks.Domain.Entities;

namespace Api.Modules.Tasks.Domain.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<TaskItem>> GetAllAsync(string userId);
    Task<IEnumerable<TaskItem>> GetByBoardAsync(string boardId, string userId);
    Task<string> CreateAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(string id, string userId);
}