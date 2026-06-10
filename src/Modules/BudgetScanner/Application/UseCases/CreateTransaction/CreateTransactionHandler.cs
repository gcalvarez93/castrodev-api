// Path: src/Modules/BudgetScanner/Application/UseCases/CreateTransaction/CreateTransactionHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Domain.Entities;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.CreateTransaction;

public sealed class CreateTransactionHandler(
    IScannerTransactionRepository repository,
    IScannerBudgetRepository budgetRepository,
    ILogger<CreateTransactionHandler> logger
) : IRequestHandler<CreateTransactionCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            return Error.Validation("Transaction.InvalidAmount", "Amount must be greater than zero.");

        var budget = await budgetRepository.GetByIdAsync(request.BudgetId, request.UserId);
        if (budget is null)
            return Error.NotFound("Budget.NotFound", "Budget not found.");

        var transaction = new ScannerTransaction
        {
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
            IsScanned       = request.IsScanned,
            CreatedAt       = DateTime.UtcNow
        };

        var id = await repository.CreateAsync(transaction);
        logger.LogInformation("Transaction {Id} created for budget {BudgetId}", id, request.BudgetId);
        return id;
    }
}