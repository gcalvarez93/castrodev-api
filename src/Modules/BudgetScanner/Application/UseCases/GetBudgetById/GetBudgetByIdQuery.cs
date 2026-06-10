// Path: src/Modules/BudgetScanner/Application/UseCases/Budgets/GetBudgetById/GetBudgetByIdQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetBudgetById;

public sealed record GetBudgetByIdQuery(string Id, string UserId)
    : IRequest<ErrorOr<ScannerBudgetDto>>;