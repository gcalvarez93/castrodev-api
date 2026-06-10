// Path: src/Modules/BudgetScanner/Domain/Repositories/IScannerBudgetRepository.cs
using Api.Modules.BudgetScanner.Domain.Entities;

namespace Api.Modules.BudgetScanner.Domain.Repositories;

public interface IScannerBudgetRepository
{
    Task<ScannerBudget?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<ScannerBudget>> GetAllAsync(string userId);
    Task<string> CreateAsync(ScannerBudget budget);
    Task UpdateAsync(ScannerBudget budget);
    Task DeleteAsync(string id, string userId);
    Task ResetRecurringBudgetsAsync(string userId);
}