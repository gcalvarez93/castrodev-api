// Path: src/Modules/Finance/Application/DTOs/BudgetResponseDto.cs
namespace Api.Modules.Finance.Application.DTOs;

public sealed record BudgetResponseDto(
    string Id,
    string CategoryId,
    string CategoryName,
    string CategoryIcon,
    decimal Amount,
    decimal Spent,
    string Month,
    DateTime CreatedAt
);