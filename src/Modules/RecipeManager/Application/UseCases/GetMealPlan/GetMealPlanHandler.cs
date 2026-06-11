// Path: src/Modules/RecipeManager/Application/UseCases/GetMealPlanHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class GetMealPlanHandler(
    IMealPlanRepository repository
) : IRequestHandler<GetMealPlanQuery, ErrorOr<MealPlanDto?>>
{
    public async Task<ErrorOr<MealPlanDto?>> Handle(
        GetMealPlanQuery request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByWeekAsync(request.UserId, request.Year, request.Week);
        if (plan is null) return (MealPlanDto?)null;

        return new MealPlanDto(
            plan.Id, plan.Year, plan.Week,
            plan.Entries.Select(e => new MealPlanEntryDto(e.DayOfWeek, e.MealType, e.RecipeId, e.RecipeName)).ToList(),
            plan.CreatedAt
        );
    }
}