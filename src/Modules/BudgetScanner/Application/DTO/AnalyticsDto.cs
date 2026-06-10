// Path: src/Modules/BudgetScanner/Application/DTOs/AnalyticsDto.cs
namespace Api.Modules.BudgetScanner.Application.DTOs;

public sealed record MonthlySummaryDto(
    int Year,
    int Month,
    decimal TotalBudgeted,
    decimal TotalSpent,
    decimal TotalRemaining,
    decimal OverallSpentPercentage,
    List<BudgetSummaryItemDto> Budgets,
    List<CategoryBreakdownDto> TopCategories,
    int AlertsTriggered
);

public sealed record BudgetSummaryItemDto(
    string BudgetId,
    string Name,
    string Category,
    string Color,
    decimal Limit,
    decimal Spent,
    decimal SpentPercentage,
    bool IsAlertTriggered
);

public sealed record CategoryBreakdownDto(
    string Category,
    decimal TotalSpent,
    int TransactionCount,
    decimal Percentage
);

public sealed record MonthComparisonDto(
    int CurrentYear,
    int CurrentMonth,
    decimal CurrentMonthSpent,
    int PreviousYear,
    int PreviousMonth,
    decimal PreviousMonthSpent,
    decimal Difference,
    decimal DifferencePercentage,
    List<CategoryComparisonDto> CategoryComparisons
);

public sealed record CategoryComparisonDto(
    string Category,
    decimal CurrentSpent,
    decimal PreviousSpent,
    decimal Difference
);