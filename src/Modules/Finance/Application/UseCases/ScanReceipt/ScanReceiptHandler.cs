// Path: src/Modules/Finance/Application/UseCases/ScanReceipt/ScanReceiptHandler.cs
using Api.Modules.Finance.Domain.Services;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.ScanReceipt;

public sealed class ScanReceiptHandler(
    IOcrService ocrService
) : IRequestHandler<ScanReceiptCommand, OcrResult>
{
    public async Task<OcrResult> Handle(
        ScanReceiptCommand request,
        CancellationToken cancellationToken)
    {
        return await ocrService.ExtractFromImageAsync(request.ImageBytes);
    }
}