// Path: src/Modules/BudgetScanner/Application/UseCases/GetMonthlySummary/GetMonthlySummaryQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Application.DTOs;

namespace Api.Modules.BudgetScanner.Application.UseCases.GetMonthlySummary;

public sealed record GetMonthlySummaryQuery(string UserId, int Year, int Month)
    : IRequest<ErrorOr<MonthlySummaryDto>>;