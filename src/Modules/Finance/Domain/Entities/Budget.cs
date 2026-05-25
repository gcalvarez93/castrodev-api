// Path: src/Modules/Finance/Domain/Budget.cs
namespace Api.Modules.Finance.Domain.Entities;

public sealed class Budget
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string CategoryId { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Month { get; init; } = string.Empty; // formato: "2026-05"
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}