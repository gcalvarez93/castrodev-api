// Path: src/Modules/Finance/Application/UseCases/ScanReceipt/ScanReceiptCommand.cs
using Api.Modules.Finance.Domain.Services;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.ScanReceipt;

public sealed record ScanReceiptCommand(byte[] ImageBytes) : IRequest<OcrResult>;