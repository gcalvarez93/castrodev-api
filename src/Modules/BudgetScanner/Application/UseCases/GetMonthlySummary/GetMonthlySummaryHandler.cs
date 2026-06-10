// Path: src/Modules/BudgetScanner/Application/UseCases/GetMonthlySummary/GetMonthlySummaryHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetMonthlySummary;

public sealed class GetMonthlySummaryHandler(
    IScannerBudgetRepository budgetRepository,
    IScannerTransactionRepository transactionRepository,
    ILogger<GetMonthlySummaryHandler> logger
) : IRequestHandler<GetMonthlySummaryQuery, ErrorOr<MonthlySummaryDto>>
{
    public async Task<ErrorOr<MonthlySummaryDto>> Handle(
        GetMonthlySummaryQuery request, CancellationToken cancellationToken)
    {
        var budgets      = (await budgetRepository.GetAllAsync(request.UserId)).ToList();
        var transactions = (await transactionRepository.GetByMonthAsync(
            request.UserId, request.Year, request.Month)).ToList();

        var budgetItems     = new List<BudgetSummaryItemDto>();
        var totalBudgeted   = 0m;
        var totalSpent      = 0m;
        var alertsTriggered = 0;

        foreach (var budget in budgets)
        {
            var spent   = await transactionRepository.GetSpentByBudgetAsync(
                budget.Id, request.UserId, request.Year, request.Month);
            var pct     = budget.Limit > 0 ? Math.Round(spent / budget.Limit * 100, 2) : 0;
            var alerted = budget.Limit > 0 && pct >= budget.AlertThreshold;

            if (alerted) alertsTriggered++;
            totalBudgeted += budget.Limit;
            totalSpent    += spent;

            budgetItems.Add(new BudgetSummaryItemDto(
                budget.Id, budget.Name, budget.Category,
                budget.Color, budget.Limit, spent, pct, alerted));
        }

        var topCategories = transactions
            .GroupBy(t => t.Category)
            .Select(g => new CategoryBreakdownDto(
                g.Key, g.Sum(t => t.Amount), g.Count(),
                totalSpent > 0 ? Math.Round(g.Sum(t => t.Amount) / totalSpent * 100, 2) : 0))
            .OrderByDescending(c => c.TotalSpent)
            .Take(5)
            .ToList();

        var overallPct = totalBudgeted > 0
            ? Math.Round(totalSpent / totalBudgeted * 100, 2) : 0;

        logger.LogInformation("Monthly summary generated for {UserId} {Year}/{Month}",
            request.UserId, request.Year, request.Month);

        return new MonthlySummaryDto(
            request.Year, request.Month,
            totalBudgeted, totalSpent,
            totalBudgeted - totalSpent,
            overallPct, budgetItems,
            topCategories, alertsTriggered
        );
    }
}