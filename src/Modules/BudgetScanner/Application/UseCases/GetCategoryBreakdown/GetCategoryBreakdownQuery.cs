// Path: src/Modules/BudgetScanner/Application/UseCases/GetCategoryBreakdown/GetCategoryBreakdownQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetCategoryBreakdown;

public sealed record GetCategoryBreakdownQuery(string UserId, int Year, int Month)
    : IRequest<ErrorOr<List<CategoryBreakdownDto>>>;