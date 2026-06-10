// Path: src/Modules/BudgetScanner/Domain/Entities/ScannerBudget.cs
namespace Api.Modules.BudgetScanner.Domain.Entities;

public sealed class ScannerBudget
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public decimal Limit { get; init; }
    public decimal Spent { get; init; }
    public string Color { get; init; } = string.Empty;
    public string Currency { get; init; } = "EUR";
    public bool IsRecurring { get; init; }
    public int AlertThreshold { get; init; } = 80;
    public bool IsAlertTriggered => Limit > 0 && (Spent / Limit * 100) >= AlertThreshold;
    public decimal RemainingAmount => Limit - Spent;
    public decimal SpentPercentage => Limit > 0 ? Math.Round(Spent / Limit * 100, 2) : 0;
    public DateTime CreatedAt { get; init; }
}