// Path: src/Modules/BudgetScanner/Application/UseCases/CreateTransaction/CreateTransactionCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.BudgetScanner.Application.UseCases.CreateTransaction;

public sealed record CreateTransactionCommand(
    string UserId,
    string BudgetId,
    string Category,
    decimal Amount,
    string Currency,
    string Description,
    string Notes,
    List<string> Tags,
    string? ReceiptImageUrl,
    string? Merchant,
    DateTime Date,
    bool IsScanned
) : IRequest<ErrorOr<string>>;