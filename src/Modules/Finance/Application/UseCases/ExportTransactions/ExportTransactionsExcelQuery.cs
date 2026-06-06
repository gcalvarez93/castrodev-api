// Path: src/Modules/Finance/Application/UseCases/ExportTransactions/ExportTransactionsExcelQuery.cs
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.ExportTransactions;

public sealed record ExportTransactionsExcelQuery(
    string UserId,
    string Month
) : IRequest<byte[]>;