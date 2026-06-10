// Path: src/Modules/BudgetScanner/Domain/Repositories/IScannerTransactionRepository.cs
using Api.Modules.BudgetScanner.Domain.Entities;

namespace Api.Modules.BudgetScanner.Domain.Repositories;

public interface IScannerTransactionRepository
{
    Task<ScannerTransaction?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<ScannerTransaction>> GetAllAsync(string userId, string? budgetId = null);
    Task<IEnumerable<ScannerTransaction>> GetByMonthAsync(string userId, int year, int month);
    Task<IEnumerable<ScannerTransaction>> GetByDateRangeAsync(string userId, DateTime from, DateTime to);
    Task<IEnumerable<ScannerTransaction>> GetByTagAsync(string userId, string tag);
    Task<decimal> GetSpentByBudgetAsync(string budgetId, string userId, int year, int month);
    Task<string> CreateAsync(ScannerTransaction transaction);
    Task UpdateAsync(ScannerTransaction transaction);
    Task DeleteAsync(string id, string userId);
}