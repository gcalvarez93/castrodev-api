// Path: src/Modules/BudgetScanner/Application/UseCases/GetTransactionById/GetTransactionByIdHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetTransactionById;

public sealed class GetTransactionByIdHandler(
    IScannerTransactionRepository repository
) : IRequestHandler<GetTransactionByIdQuery, ErrorOr<ScannerTransactionDto>>
{
    public async Task<ErrorOr<ScannerTransactionDto>> Handle(
        GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var t = await repository.GetByIdAsync(request.Id, request.UserId);
        if (t is null) return Error.NotFound("Transaction.NotFound", "Transaction not found.");

        return new ScannerTransactionDto(
            t.Id, t.BudgetId, t.Category, t.Amount, t.Currency,
            t.Description, t.Notes, t.Tags,
            t.ReceiptImageUrl, t.Merchant,
            t.Date, t.CreatedAt, t.IsScanned
        );
    }
}