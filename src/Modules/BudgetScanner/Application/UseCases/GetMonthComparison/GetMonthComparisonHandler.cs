// Path: src/Modules/BudgetScanner/Application/UseCases/GetMonthComparison/GetMonthComparisonHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetMonthComparison;

public sealed class GetMonthComparisonHandler(
    IScannerTransactionRepository repository
) : IRequestHandler<GetMonthComparisonQuery, ErrorOr<MonthComparisonDto>>
{
    public async Task<ErrorOr<MonthComparisonDto>> Handle(
        GetMonthComparisonQuery request, CancellationToken cancellationToken)
    {
        var prevDate = new DateTime(request.Year, request.Month, 1).AddMonths(-1);

        var current  = (await repository.GetByMonthAsync(
            request.UserId, request.Year, request.Month)).ToList();
        var previous = (await repository.GetByMonthAsync(
            request.UserId, prevDate.Year, prevDate.Month)).ToList();

        var currentSpent  = current.Sum(t => t.Amount);
        var previousSpent = previous.Sum(t => t.Amount);
        var diff          = currentSpent - previousSpent;
        var diffPct       = previousSpent > 0 ? Math.Round(diff / previousSpent * 100, 2) : 0;

        var allCategories = current.Select(t => t.Category)
            .Union(previous.Select(t => t.Category))
            .Distinct();

        var categoryComparisons = allCategories.Select(cat => new CategoryComparisonDto(
            cat,
            current.Where(t => t.Category == cat).Sum(t => t.Amount),
            previous.Where(t => t.Category == cat).Sum(t => t.Amount),
            current.Where(t => t.Category == cat).Sum(t => t.Amount) -
            previous.Where(t => t.Category == cat).Sum(t => t.Amount)
        )).OrderByDescending(c => c.CurrentSpent).ToList();

        return new MonthComparisonDto(
            request.Year, request.Month, currentSpent,
            prevDate.Year, prevDate.Month, previousSpent,
            diff, diffPct, categoryComparisons
        );
    }
}