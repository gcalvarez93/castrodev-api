// Path: src/Modules/BudgetScanner/Application/UseCases/UpdateTransaction/UpdateTransactionCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.BudgetScanner.Application.UseCases.UpdateTransaction;

public sealed record UpdateTransactionCommand(
    string Id,
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
    DateTime Date
) : IRequest<ErrorOr<Updated>>;