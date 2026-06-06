// Path: src/Modules/Finance/Infrastructure/Services/PdfService.cs
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace Api.Modules.Finance.Infrastructure.Services;

public sealed class PdfService : IPdfService
{
    public Task<byte[]> GenerateTransactionReportAsync(
        string userName,
        string month,
        IEnumerable<Transaction> transactions,
        decimal balance)
    {
        using var stream = new MemoryStream();
        using var writer = new PdfWriter(stream);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);

        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        // Título
        document.Add(new Paragraph("Reporte de transacciones")
            .SetFont(boldFont)
            .SetFontSize(20)
            .SetTextAlignment(TextAlignment.CENTER));

        document.Add(new Paragraph($"Usuario: {userName}")
            .SetFont(normalFont)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.CENTER));

        document.Add(new Paragraph($"Mes: {month}")
            .SetFont(normalFont)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.CENTER));

        document.Add(new Paragraph("\n"));

        // Balance
        document.Add(new Paragraph($"Balance total: {balance:C2}")
            .SetFont(boldFont)
            .SetFontSize(14));

        document.Add(new Paragraph("\n"));

        // Tabla
        var table = new Table(5).UseAllAvailableWidth();

        foreach (var header in new[] { "Fecha", "Descripción", "Categoría", "Tipo", "Importe" })
        {
            table.AddHeaderCell(new Cell()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .Add(new Paragraph(header).SetFont(boldFont)));
        }

        foreach (var t in transactions.OrderByDescending(t => t.Date))
        {
            table.AddCell(new Cell().Add(new Paragraph(t.Date.ToString("dd/MM/yyyy")).SetFont(normalFont)));
            table.AddCell(new Cell().Add(new Paragraph(t.Description).SetFont(normalFont)));
            table.AddCell(new Cell().Add(new Paragraph(t.CategoryId).SetFont(normalFont)));
            table.AddCell(new Cell().Add(new Paragraph(t.Type == "income" ? "Ingreso" : "Gasto").SetFont(normalFont)));
            table.AddCell(new Cell().Add(new Paragraph($"{t.Amount:C2}")
                .SetFont(normalFont)
                .SetFontColor(t.Type == "income" ? ColorConstants.GREEN : ColorConstants.RED)));
        }

        document.Add(table);
        document.Close();

        return Task.FromResult(stream.ToArray());
    }
}