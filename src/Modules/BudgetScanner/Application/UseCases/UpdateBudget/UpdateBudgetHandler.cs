// Path: src/Modules/BudgetScanner/Application/UseCases/UpdateBudget/UpdateBudgetHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Domain.Entities;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.UpdateBudget;

public sealed class UpdateBudgetHandler(
    IScannerBudgetRepository repository
) : IRequestHandler<UpdateBudgetCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(
        UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Budget.NotFound", "Budget not found.");

        if (request.Limit <= 0)
            return Error.Validation("Budget.InvalidLimit", "Limit must be greater than zero.");

        var updated = new ScannerBudget
        {
            Id             = request.Id,
            UserId         = request.UserId,
            Name           = request.Name,
            Category       = request.Category,
            Limit          = request.Limit,
            Color          = request.Color,
            Currency       = request.Currency,
            IsRecurring    = request.IsRecurring,
            AlertThreshold = request.AlertThreshold,
            CreatedAt      = existing.CreatedAt
        };

        await repository.UpdateAsync(updated);
        return Result.Updated;
    }
}