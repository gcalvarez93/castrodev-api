// Path: src/Modules/BudgetScanner/Application/DTOs/ScannerBudgetDto.cs
namespace Api.Modules.BudgetScanner.Application.DTOs;

public sealed record ScannerBudgetDto(
    string Id,
    string Name,
    string Category,
    decimal Limit,
    decimal Spent,
    decimal RemainingAmount,
    decimal SpentPercentage,
    string Color,
    string Currency,
    bool IsRecurring,
    int AlertThreshold,
    bool IsAlertTriggered,
    DateTime CreatedAt
);