// Path: src/Modules/BudgetScanner/Application/DTOs/ScannerTransactionDto.cs
namespace Api.Modules.BudgetScanner.Application.DTOs;

public sealed record ScannerTransactionDto(
    string Id,
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
    DateTime CreatedAt,
    bool IsScanned
);