// Path: src/Modules/BudgetScanner/Application/UseCases/GetBudgetById/GetBudgetByIdHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetBudgetById;

public sealed class GetBudgetByIdHandler(
    IScannerBudgetRepository budgetRepository,
    IScannerTransactionRepository transactionRepository
) : IRequestHandler<GetBudgetByIdQuery, ErrorOr<ScannerBudgetDto>>
{
    public async Task<ErrorOr<ScannerBudgetDto>> Handle(
        GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        var budget = await budgetRepository.GetByIdAsync(request.Id, request.UserId);
        if (budget is null) return Error.NotFound("Budget.NotFound", "Budget not found.");

        var now   = DateTime.UtcNow;
        var spent = await transactionRepository.GetSpentByBudgetAsync(
            budget.Id, request.UserId, now.Year, now.Month);

        return new ScannerBudgetDto(
            budget.Id, budget.Name, budget.Category,
            budget.Limit, spent,
            budget.Limit - spent,
            budget.Limit > 0 ? Math.Round(spent / budget.Limit * 100, 2) : 0,
            budget.Color, budget.Currency,
            budget.IsRecurring, budget.AlertThreshold,
            budget.Limit > 0 && (spent / budget.Limit * 100) >= budget.AlertThreshold,
            budget.CreatedAt
        );
    }
}