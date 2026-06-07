// Path: src/Common/Domain/Entities/User.cs
namespace Api.Common.Domain.Entities;

public sealed class User
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhotoUrl { get; init; } = string.Empty;
    public string Language { get; init; } = "es";
    public bool Notifications { get; init; } = true;
    public bool NotificationsGeneral { get; init; } = true;
    public bool NotificationsTransactions { get; init; } = true;
    public bool NotificationsBudgets { get; init; } = false;
    public bool NotificationsReports { get; init; } = false;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}