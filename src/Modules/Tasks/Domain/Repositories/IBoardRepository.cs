// Path: src/Modules/Tasks/Domain/Repositories/IBoardRepository.cs
using Api.Modules.Tasks.Domain.Entities;

namespace Api.Modules.Tasks.Domain.Repositories;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<Board>> GetAllAsync(string userId);
    Task<string> CreateAsync(Board board);
    Task DeleteAsync(string id, string userId);
}