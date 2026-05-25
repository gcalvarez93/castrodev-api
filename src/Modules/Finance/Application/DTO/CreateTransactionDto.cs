// Path: src/Modules/Finance/Application/DTOs/CreateTransactionDto.cs
namespace Api.Modules.Finance.Application.DTOs;

public sealed record CreateTransactionDto(
    decimal Amount,
    string Type,
    string CategoryId,
    string Description,
    DateTime Date
);