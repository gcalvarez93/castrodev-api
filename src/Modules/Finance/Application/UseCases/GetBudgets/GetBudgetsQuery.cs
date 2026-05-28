// Path: src/Modules/Finance/Application/UseCases/GetBudgets/GetBudgetsQuery.cs
using Api.Modules.Finance.Application.DTOs;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetBudgets;

public sealed record GetBudgetsQuery(string UserId, string Month) : IRequest<IEnumerable<BudgetResponseDto>>;