// Path: src/Modules/Finance/Application/UseCases/ExportTransactions/ExportTransactionsExcelHandler.cs
using Api.Modules.Finance.Domain.Repositories;
using Api.Modules.Finance.Domain.Services;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.ExportTransactions;

public sealed class ExportTransactionsExcelHandler(
    ITransactionRepository transactionRepository,
    IExcelService excelService
) : IRequestHandler<ExportTransactionsExcelQuery, byte[]>
{
    public async Task<byte[]> Handle(
        ExportTransactionsExcelQuery request,
        CancellationToken cancellationToken)
    {
        var parts = request.Month.Split('-');
        var year = int.Parse(parts[0]);
        var month = int.Parse(parts[1]);

        var transactions = await transactionRepository.GetByMonthAsync(request.UserId, year, month);

        return await excelService.GenerateTransactionReportAsync(
            request.Month,
            transactions
        );
    }
}