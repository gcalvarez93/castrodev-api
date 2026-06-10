// Path: src/Modules/BudgetScanner/Application/UseCases/DeleteTransaction/DeleteTransactionHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.DeleteTransaction;

public sealed class DeleteTransactionHandler(
    IScannerTransactionRepository repository
) : IRequestHandler<DeleteTransactionCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Transaction.NotFound", "Transaction not found.");

        await repository.DeleteAsync(request.Id, request.UserId);
        return Result.Deleted;
    }
}