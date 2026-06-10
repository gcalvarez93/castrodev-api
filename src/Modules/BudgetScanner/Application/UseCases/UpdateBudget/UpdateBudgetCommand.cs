// Path: src/Modules/BudgetScanner/Application/UseCases/UpdateBudget/UpdateBudgetCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.BudgetScanner.Application.UseCases.UpdateBudget;

public sealed record UpdateBudgetCommand(
    string Id,
    string UserId,
    string Name,
    string Category,
    decimal Limit,
    string Color,
    string Currency,
    bool IsRecurring,
    int AlertThreshold
) : IRequest<ErrorOr<Updated>>;