// Path: src/Modules/BudgetScanner/Application/UseCases/ResetRecurringBudgets/ResetRecurringBudgetsCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.BudgetScanner.Application.UseCases.ResetRecurringBudgets;

public sealed record ResetRecurringBudgetsCommand(string UserId) : IRequest<ErrorOr<Updated>>;