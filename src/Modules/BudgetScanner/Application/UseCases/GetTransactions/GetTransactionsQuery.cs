// Path: src/Modules/BudgetScanner/Application/UseCases/GetTransactions/GetTransactionsQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetTransactions;

public sealed record GetTransactionsQuery(string UserId, string? BudgetId = null)
    : IRequest<ErrorOr<List<ScannerTransactionDto>>>;