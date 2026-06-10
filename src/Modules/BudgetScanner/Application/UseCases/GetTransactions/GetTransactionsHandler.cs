// Path: src/Modules/BudgetScanner/Application/UseCases/GetTransactions/GetTransactionsHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetTransactions;

public sealed class GetTransactionsHandler(
    IScannerTransactionRepository repository
) : IRequestHandler<GetTransactionsQuery, ErrorOr<List<ScannerTransactionDto>>>
{
    public async Task<ErrorOr<List<ScannerTransactionDto>>> Handle(
        GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await repository.GetAllAsync(request.UserId, request.BudgetId);
        return transactions.Select(t => new ScannerTransactionDto(
            t.Id, t.BudgetId, t.Category, t.Amount, t.Currency,
            t.Description, t.Notes, t.Tags,
            t.ReceiptImageUrl, t.Merchant,
            t.Date, t.CreatedAt, t.IsScanned
        )).ToList();
    }
}