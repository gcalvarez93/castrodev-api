// Path: src/Modules/BudgetScanner/Application/UseCases/GetTransactionById/GetTransactionByIdQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetTransactionById;

public sealed record GetTransactionByIdQuery(string Id, string UserId)
    : IRequest<ErrorOr<ScannerTransactionDto>>;