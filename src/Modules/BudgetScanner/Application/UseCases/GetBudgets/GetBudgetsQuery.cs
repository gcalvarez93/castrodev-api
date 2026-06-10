// Path: src/Modules/BudgetScanner/Application/UseCases/GetBudgets/GetBudgetsQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetBudgets;

public sealed record GetBudgetsQuery(string UserId) : IRequest<ErrorOr<List<ScannerBudgetDto>>>;