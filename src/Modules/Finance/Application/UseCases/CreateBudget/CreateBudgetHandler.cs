// Path: src/Modules/Finance/Application/UseCases/CreateBudget/CreateBudgetHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.CreateBudget;

public sealed class CreateBudgetHandler(
    IBudgetRepository repository
) : IRequestHandler<CreateBudgetCommand, BudgetResponseDto>
{
    public async Task<BudgetResponseDto> Handle(
        CreateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var budget = new Budget
        {
            UserId = request.UserId,
            CategoryId = request.Dto.CategoryId,
            Amount = request.Dto.Amount,
            Month = request.Dto.Month
        };

        var id = await repository.CreateAsync(budget);

        return new BudgetResponseDto(
            id,
            budget.CategoryId,
            budget.Amount,
            budget.Month,
            budget.CreatedAt
        );
    }
}