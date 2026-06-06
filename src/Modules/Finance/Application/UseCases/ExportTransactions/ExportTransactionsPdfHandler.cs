// Path: src/Modules/Finance/Application/UseCases/ExportTransactions/ExportTransactionsPdfHandler.cs
using Api.Modules.Finance.Domain.Repositories;
using Api.Modules.Finance.Domain.Services;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.ExportTransactions;

public sealed class ExportTransactionsPdfHandler(
    ITransactionRepository transactionRepository,
    IPdfService pdfService
) : IRequestHandler<ExportTransactionsPdfQuery, byte[]>
{
    public async Task<byte[]> Handle(
        ExportTransactionsPdfQuery request,
        CancellationToken cancellationToken)
    {
        var parts = request.Month.Split('-');
        var year = int.Parse(parts[0]);
        var month = int.Parse(parts[1]);

        var transactions = await transactionRepository.GetByMonthAsync(request.UserId, year, month);
        var balance = await transactionRepository.GetBalanceAsync(request.UserId);

        return await pdfService.GenerateTransactionReportAsync(
            request.UserName,
            request.Month,
            transactions,
            balance
        );
    }
}