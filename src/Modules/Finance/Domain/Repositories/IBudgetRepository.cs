// Path: src/Modules/Finance/Domain/Repositories/IBudgetRepository.cs
using Api.Modules.Finance.Domain.Entities;

namespace Api.Modules.Finance.Domain.Repositories;

public interface IBudgetRepository
{
    Task<Budget?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<Budget>> GetAllAsync(string userId);
    Task<IEnumerable<Budget>> GetByMonthAsync(string userId, string month);
    Task<string> CreateAsync(Budget budget);
    Task UpdateAsync(Budget budget);
    Task DeleteAsync(string id, string userId);
}