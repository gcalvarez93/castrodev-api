// Path: src/Modules/BudgetScanner/Application/UseCases/ScanReceipt/ScanReceiptCommand.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.ScanReceipt;

public sealed record ScanReceiptCommand(string UserId, string ImageBase64)
    : IRequest<ErrorOr<OcrResultDto>>;