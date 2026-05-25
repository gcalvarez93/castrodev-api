// Path: src/Modules/Finance/Application/DTOs/TransactionResponseDto.cs
namespace Api.Modules.Finance.Application.DTOs;

public sealed record TransactionResponseDto(
    string Id,
    decimal Amount,
    string Type,
    string CategoryId,
    string Description,
    DateTime Date,
    DateTime CreatedAt
);