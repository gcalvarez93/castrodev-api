// Path: src/Modules/BudgetScanner/Application/UseCases/GetBudgets/GetBudgetsHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetBudgets;

public sealed class GetBudgetsHandler(
    IScannerBudgetRepository budgetRepository,
    IScannerTransactionRepository transactionRepository,
    ILogger<GetBudgetsHandler> logger
) : IRequestHandler<GetBudgetsQuery, ErrorOr<List<ScannerBudgetDto>>>
{
    public async Task<ErrorOr<List<ScannerBudgetDto>>> Handle(
        GetBudgetsQuery request, CancellationToken cancellationToken)
    {
        var budgets = await budgetRepository.GetAllAsync(request.UserId);
        var now     = DateTime.UtcNow;
        var result  = new List<ScannerBudgetDto>();

        foreach (var budget in budgets)
        {
            var spent = await transactionRepository.GetSpentByBudgetAsync(
                budget.Id, request.UserId, now.Year, now.Month);

            result.Add(new ScannerBudgetDto(
                budget.Id, budget.Name, budget.Category,
                budget.Limit, spent,
                budget.Limit - spent,
                budget.Limit > 0 ? Math.Round(spent / budget.Limit * 100, 2) : 0,
                budget.Color, budget.Currency,
                budget.IsRecurring, budget.AlertThreshold,
                budget.Limit > 0 && (spent / budget.Limit * 100) >= budget.AlertThreshold,
                budget.CreatedAt
            ));
        }

        logger.LogInformation("Retrieved {Count} budgets for user {UserId}", result.Count, request.UserId);
        return result;
    }
}