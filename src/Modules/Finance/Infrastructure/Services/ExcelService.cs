// Path: src/Modules/Finance/Infrastructure/Services/ExcelService.cs
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Services;
using ClosedXML.Excel;

namespace Api.Modules.Finance.Infrastructure.Services;

public sealed class ExcelService : IExcelService
{
    public Task<byte[]> GenerateTransactionReportAsync(
        string month,
        IEnumerable<Transaction> transactions)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add($"Transacciones {month}");

        // Cabeceras
        worksheet.Cell(1, 1).Value = "Fecha";
        worksheet.Cell(1, 2).Value = "Descripción";
        worksheet.Cell(1, 3).Value = "Categoría";
        worksheet.Cell(1, 4).Value = "Tipo";
        worksheet.Cell(1, 5).Value = "Importe";

        // Estilo cabeceras
        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

        // Filas
        var row = 2;
        foreach (var t in transactions.OrderByDescending(t => t.Date))
        {
            worksheet.Cell(row, 1).Value = t.Date.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 2).Value = t.Description;
            worksheet.Cell(row, 3).Value = t.CategoryId;
            worksheet.Cell(row, 4).Value = t.Type == "income" ? "Ingreso" : "Gasto";
            worksheet.Cell(row, 5).Value = (double)t.Amount;

            // Color según tipo
            worksheet.Cell(row, 5).Style.Font.FontColor =
                t.Type == "income" ? XLColor.Green : XLColor.Red;

            row++;
        }

        // Ajustar ancho columnas
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Task.FromResult(stream.ToArray());
    }
}