// Path: src/Modules/Finance/Domain/Services/IPdfService.cs
using Api.Modules.Finance.Domain.Entities;

namespace Api.Modules.Finance.Domain.Services;

public interface IPdfService
{
    Task<byte[]> GenerateTransactionReportAsync(
        string userName,
        string month,
        IEnumerable<Transaction> transactions,
        decimal balance
    );
}