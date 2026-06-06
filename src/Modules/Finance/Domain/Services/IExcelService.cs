// Path: src/Modules/Finance/Domain/Services/IExcelService.cs
using Api.Modules.Finance.Domain.Entities;

namespace Api.Modules.Finance.Domain.Services;

public interface IExcelService
{
    Task<byte[]> GenerateTransactionReportAsync(
        string month,
        IEnumerable<Transaction> transactions
    );
}