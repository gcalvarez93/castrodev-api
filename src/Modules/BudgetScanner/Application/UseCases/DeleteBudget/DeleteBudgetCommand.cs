// Path: src/Modules/BudgetScanner/Application/UseCases/DeleteBudget/DeleteBudgetCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.BudgetScanner.Application.UseCases.DeleteBudget;

public sealed record DeleteBudgetCommand(string Id, string UserId) : IRequest<ErrorOr<Deleted>>;