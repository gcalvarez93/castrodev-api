// Path: src/Modules/Finance/Application/UseCases/GetBudgets/GetBudgetsHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetBudgets;

public sealed class GetBudgetsHandler(
    IBudgetRepository budgetRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository
) : IRequestHandler<GetBudgetsQuery, IEnumerable<BudgetResponseDto>>
{
    public async Task<IEnumerable<BudgetResponseDto>> Handle(
        GetBudgetsQuery request,
        CancellationToken cancellationToken)
    {
        var budgets = await budgetRepository.GetByMonthAsync(request.UserId, request.Month);

        // Parsear mes y año del formato "2026-06"
        var parts = request.Month.Split('-');
        var year = int.Parse(parts[0]);
        var month = int.Parse(parts[1]);

        // Obtener todas las transacciones del mes
        var transactions = await transactionRepository.GetByMonthAsync(
            request.UserId, year, month);

        // Obtener categorías para nombre e icono
        var categories = await categoryRepository.GetAllAsync(request.UserId);
        var categoryMap = categories.ToDictionary(c => c.Id, c => c);

        return budgets.Select(b =>
        {
            // Sumar gastos de esa categoría en el mes
            var spent = transactions
                .Where(t => t.CategoryId == b.CategoryId && t.Type == "expense")
                .Sum(t => t.Amount);

            categoryMap.TryGetValue(b.CategoryId, out var category);

            return new BudgetResponseDto(
                b.Id,
                b.CategoryId,
                category?.Name ?? string.Empty,
                category?.Icon ?? "💰",
                b.Amount,
                spent,
                b.Month,
                b.CreatedAt
            );
        });
    }
}