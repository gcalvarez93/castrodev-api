// Path: src/Modules/Finance/Application/UseCases/CreateBudget/CreateBudgetHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.CreateBudget;

public sealed class CreateBudgetHandler(
    IBudgetRepository budgetRepository,
    ICategoryRepository categoryRepository
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

        var id = await budgetRepository.CreateAsync(budget);

        var category = await categoryRepository.GetByIdAsync(
            request.Dto.CategoryId, request.UserId);

        return new BudgetResponseDto(
            id,
            budget.CategoryId,
            category?.Name ?? string.Empty,
            category?.Icon ?? "💰",
            budget.Amount,
            0,
            budget.Month,
            budget.CreatedAt
        );
    }
}