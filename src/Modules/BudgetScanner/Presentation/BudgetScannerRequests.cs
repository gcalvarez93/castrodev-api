// Path: src/Modules/BudgetScanner/Presentation/BudgetScannerRequests.cs
namespace Api.Modules.BudgetScanner.Presentation;

public sealed record CreateBudgetRequest(
    string Name,
    string Category,
    decimal Limit,
    string Color,
    string Currency,
    bool IsRecurring,
    int AlertThreshold
);

public sealed record UpdateBudgetRequest(
    string Name,
    string Category,
    decimal Limit,
    string Color,
    string Currency,
    bool IsRecurring,
    int AlertThreshold
);

public sealed record CreateTransactionRequest(
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
);

public sealed record UpdateTransactionRequest(
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
);

public sealed record ScanReceiptRequest(string ImageBase64);