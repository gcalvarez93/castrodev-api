// Path: src/Modules/BudgetScanner/Application/UseCases/GetMonthComparison/GetMonthComparisonQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetMonthComparison;

public sealed record GetMonthComparisonQuery(string UserId, int Year, int Month)
    : IRequest<ErrorOr<MonthComparisonDto>>;