// Path: src/Modules/BudgetScanner/Application/DTOs/OcrResultDto.cs
namespace Api.Modules.BudgetScanner.Application.DTOs;

public sealed record OcrResultDto(
    decimal? Amount,
    string? Merchant,
    DateTime? Date,
    string? Currency,
    string RawText
);