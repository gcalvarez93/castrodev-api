// Path: src/Modules/BudgetScanner/Application/UseCases/DeleteTransaction/DeleteTransactionCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.BudgetScanner.Application.UseCases.DeleteTransaction;

public sealed record DeleteTransactionCommand(string Id, string UserId) : IRequest<ErrorOr<Deleted>>;