// Path: src/Modules/BudgetScanner/Application/UseCases/ScanReceipt/ScanReceiptHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;
using Api.Modules.Finance.Domain.Services;

namespace Api.Modules.BudgetScanner.Application.UseCases.ScanReceipt;

public sealed class ScanReceiptHandler(
    IOcrService ocrService,
    ILogger<ScanReceiptHandler> logger
) : IRequestHandler<ScanReceiptCommand, ErrorOr<OcrResultDto>>
{
    public async Task<ErrorOr<OcrResultDto>> Handle(
        ScanReceiptCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ImageBase64))
            return Error.Validation("Receipt.EmptyImage", "Image data cannot be empty.");

        byte[] imageBytes;
        try
        {
            imageBytes = Convert.FromBase64String(request.ImageBase64);
        }
        catch
        {
            return Error.Validation("Receipt.InvalidImage", "Image data is not valid base64.");
        }

        var ocrResult = await ocrService.ExtractFromImageAsync(imageBytes);

        if (ocrResult is null)
            return Error.Failure("Receipt.OcrFailed", "Could not extract data from the image.");

        logger.LogInformation("Receipt scanned for user {UserId}: amount={Amount}, commerce={Commerce}",
            request.UserId, ocrResult.Amount, ocrResult.Commerce);

        return new OcrResultDto(
            ocrResult.Amount,
            ocrResult.Commerce,
            ocrResult.Date,
            null,
            string.Empty
        );
    }
}