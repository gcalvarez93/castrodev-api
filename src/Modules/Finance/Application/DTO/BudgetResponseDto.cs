// Path: src/Modules/Finance/Application/DTOs/BudgetResponseDto.cs
namespace Api.Modules.Finance.Application.DTOs;

public sealed record BudgetResponseDto(
    string Id,
    string CategoryId,
    decimal Amount,
    string Month,
    DateTime CreatedAt
);