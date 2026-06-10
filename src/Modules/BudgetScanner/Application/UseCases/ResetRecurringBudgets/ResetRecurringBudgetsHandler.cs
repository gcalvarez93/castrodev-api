// Path: src/Modules/BudgetScanner/Application/UseCases/ResetRecurringBudgets/ResetRecurringBudgetsHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.ResetRecurringBudgets;

public sealed class ResetRecurringBudgetsHandler(
    IScannerBudgetRepository repository,
    ILogger<ResetRecurringBudgetsHandler> logger
) : IRequestHandler<ResetRecurringBudgetsCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(
        ResetRecurringBudgetsCommand request, CancellationToken cancellationToken)
    {
        await repository.ResetRecurringBudgetsAsync(request.UserId);
        logger.LogInformation("Recurring budgets reset for user {UserId}", request.UserId);
        return Result.Updated;
    }
}