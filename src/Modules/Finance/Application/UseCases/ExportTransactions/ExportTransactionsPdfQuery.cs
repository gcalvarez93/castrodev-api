// Path: src/Modules/Finance/Application/UseCases/ExportTransactions/ExportTransactionsPdfQuery.cs
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.ExportTransactions;

public sealed record ExportTransactionsPdfQuery(
    string UserId,
    string UserName,
    string Month
) : IRequest<byte[]>;