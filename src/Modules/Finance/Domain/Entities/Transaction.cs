// Path: src/Modules/Finance/Domain/Transaction.cs
namespace Api.Modules.Finance.Domain.Entities;

public sealed class Transaction
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Type { get; init; } = string.Empty; // "income" | "expense"
    public string CategoryId { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime Date { get; init; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}