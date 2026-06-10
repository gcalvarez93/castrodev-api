// Path: src/Modules/BudgetScanner/Application/UseCases/UpdateTransaction/UpdateTransactionHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Domain.Entities;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.UpdateTransaction;

public sealed class UpdateTransactionHandler(
    IScannerTransactionRepository repository
) : IRequestHandler<UpdateTransactionCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(
        UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Transaction.NotFound", "Transaction not found.");

        if (request.Amount <= 0)
            return Error.Validation("Transaction.InvalidAmount", "Amount must be greater than zero.");

        var updated = new ScannerTransaction
        {
            Id              = request.Id,
            UserId          = request.UserId,
            BudgetId        = request.BudgetId,
            Category        = request.Category,
            Amount          = request.Amount,
            Currency        = request.Currency,
            Description     = request.Description,
            Notes           = request.Notes,
            Tags            = request.Tags,
            ReceiptImageUrl = request.ReceiptImageUrl,
            Merchant        = request.Merchant,
            Date            = request.Date,
            IsScanned       = existing.IsScanned,
            CreatedAt       = existing.CreatedAt
        };

        await repository.UpdateAsync(updated);
        return Result.Updated;
    }
}