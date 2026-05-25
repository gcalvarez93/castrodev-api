// Path: src/Modules/Finance/Domain/Interfaces/ITransactionRepository.cs
using Api.Modules.Finance.Domain.Entities;

namespace Api.Modules.Finance.Domain.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<Transaction>> GetAllAsync(string userId);
    Task<IEnumerable<Transaction>> GetByMonthAsync(string userId, int year, int month);
    Task<string> CreateAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(string id, string userId);
    Task<decimal> GetBalanceAsync(string userId);
}