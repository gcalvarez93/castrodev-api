// Path: src/Modules/Finance/Application/UseCases/GetTransactions/GetTransactionsQuery.cs
using Api.Modules.Finance.Application.DTOs;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetTransactions;

public sealed record GetTransactionsQuery(string UserId) : IRequest<IEnumerable<TransactionResponseDto>>;