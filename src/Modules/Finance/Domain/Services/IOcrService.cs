// Path: src/Modules/Finance/Domain/Services/IOcrService.cs
namespace Api.Modules.Finance.Domain.Services;

public sealed record OcrResult(
    decimal? Amount,
    DateTime? Date,
    string? Commerce
);

public interface IOcrService
{
    Task<OcrResult> ExtractFromImageAsync(byte[] imageBytes);
}