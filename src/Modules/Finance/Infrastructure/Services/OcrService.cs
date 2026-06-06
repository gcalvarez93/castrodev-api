// Path: src/Modules/Finance/Infrastructure/Services/OcrService.cs
using Api.Modules.Finance.Domain.Services;
using Google.Cloud.Vision.V1;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Api.Modules.Finance.Infrastructure.Services;

public sealed class OcrService : IOcrService
{
    public async Task<OcrResult> ExtractFromImageAsync(byte[] imageBytes)
    {
        var client = ImageAnnotatorClient.Create();
        var image = Image.FromBytes(imageBytes);
        var response = await client.DetectTextAsync(image);

        if (response is null || !response.Any())
            return new OcrResult(null, null, null);

        var fullText = response.First().Description;

        var amount = ExtractAmount(fullText);
        var date = ExtractDate(fullText);
        var commerce = ExtractCommerce(response);

        return new OcrResult(amount, date, commerce);
    }

    private static decimal? ExtractAmount(string text)
    {
        // Busca patrones como "12,50", "12.50", "12,50 €", "€12.50"
        var match = Regex.Match(text, @"(\d+[.,]\d{2})\s*€?|€\s*(\d+[.,]\d{2})");
        if (!match.Success) return null;

        var raw = (match.Groups[1].Value + match.Groups[2].Value)
            .Replace(",", ".");

        return decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            ? result
            : null;
    }

    private static DateTime? ExtractDate(string text)
    {
        // Busca patrones como "24/05/2026", "24-05-2026", "24.05.2026"
        var match = Regex.Match(text, @"\d{2}[/\-\.]\d{2}[/\-\.]\d{4}");
        if (!match.Success) return null;

        var normalized = match.Value.Replace("-", "/").Replace(".", "/");
        return DateTime.TryParseExact(normalized, "dd/MM/yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;
    }

    private static string? ExtractCommerce(IReadOnlyList<EntityAnnotation> annotations)
    {
        // La primera anotación es el texto completo, la segunda suele ser el nombre del comercio
        return annotations.Count > 1
            ? annotations[1].Description.Trim()
            : null;
    }
}