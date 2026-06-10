// Path: src/Modules/BudgetScanner/Domain/Entities/ScannerTransaction.cs
namespace Api.Modules.BudgetScanner.Domain.Entities;

public sealed class ScannerTransaction
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string BudgetId { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "EUR";
    public string Description { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    public string? ReceiptImageUrl { get; init; }
    public string? Merchant { get; init; }
    public DateTime Date { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsScanned { get; init; }
}