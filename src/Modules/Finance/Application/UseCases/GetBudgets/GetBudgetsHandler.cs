// Path: src/Modules/Finance/Application/UseCases/GetBudgets/GetBudgetsHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetBudgets;

public sealed class GetBudgetsHandler(
    IBudgetRepository repository
) : IRequestHandler<GetBudgetsQuery, IEnumerable<BudgetResponseDto>>
{
    public async Task<IEnumerable<BudgetResponseDto>> Handle(
        GetBudgetsQuery request,
        CancellationToken cancellationToken)
    {
        var budgets = await repository.GetByMonthAsync(request.UserId, request.Month);

        return budgets.Select(b => new BudgetResponseDto(
            b.Id,
            b.CategoryId,
            b.Amount,
            b.Month,
            b.CreatedAt
        ));
    }
}