// Path: src/Modules/BudgetScanner/Application/UseCases/CreateBudget/CreateBudgetCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.BudgetScanner.Application.UseCases.CreateBudget;

public sealed record CreateBudgetCommand(
    string UserId,
    string Name,
    string Category,
    decimal Limit,
    string Color,
    string Currency,
    bool IsRecurring,
    int AlertThreshold
) : IRequest<ErrorOr<string>>;