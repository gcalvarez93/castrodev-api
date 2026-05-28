// Path: src/Modules/Finance/Application/DTOs/CreateBudgetDto.cs
namespace Api.Modules.Finance.Application.DTOs;

public sealed record CreateBudgetDto(
    string CategoryId,
    decimal Amount,
    string Month
);