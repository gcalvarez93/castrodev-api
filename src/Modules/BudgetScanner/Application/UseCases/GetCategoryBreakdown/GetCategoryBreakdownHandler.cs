// Path: src/Modules/BudgetScanner/Application/UseCases/GetCategoryBreakdown/GetCategoryBreakdownHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetCategoryBreakdown;

public sealed class GetCategoryBreakdownHandler(
    IScannerTransactionRepository repository
) : IRequestHandler<GetCategoryBreakdownQuery, ErrorOr<List<CategoryBreakdownDto>>>
{
    public async Task<ErrorOr<List<CategoryBreakdownDto>>> Handle(
        GetCategoryBreakdownQuery request, CancellationToken cancellationToken)
    {
        var transactions = (await repository.GetByMonthAsync(
            request.UserId, request.Year, request.Month)).ToList();

        var totalSpent = transactions.Sum(t => t.Amount);

        return transactions
            .GroupBy(t => t.Category)
            .Select(g => new CategoryBreakdownDto(
                g.Key, g.Sum(t => t.Amount), g.Count(),
                totalSpent > 0 ? Math.Round(g.Sum(t => t.Amount) / totalSpent * 100, 2) : 0))
            .OrderByDescending(c => c.TotalSpent)
            .ToList();
    }
}