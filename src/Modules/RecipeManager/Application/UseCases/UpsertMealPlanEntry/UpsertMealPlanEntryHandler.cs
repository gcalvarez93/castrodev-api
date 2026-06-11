// Path: src/Modules/RecipeManager/Application/UseCases/UpsertMealPlanEntryHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class UpsertMealPlanEntryHandler(
    IMealPlanRepository repository
) : IRequestHandler<UpsertMealPlanEntryCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        UpsertMealPlanEntryCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByWeekAsync(request.UserId, request.Year, request.Week);

        var entries = existing?.Entries.ToList() ?? [];
        entries.RemoveAll(e => e.DayOfWeek == request.DayOfWeek && e.MealType == request.MealType);
        entries.Add(new MealPlanEntry
        {
            DayOfWeek  = request.DayOfWeek,
            MealType   = request.MealType,
            RecipeId   = request.RecipeId,
            RecipeName = request.RecipeName
        });

        var plan = new MealPlan
        {
            Id        = existing?.Id ?? string.Empty,
            UserId    = request.UserId,
            Year      = request.Year,
            Week      = request.Week,
            Entries   = entries,
            CreatedAt = existing?.CreatedAt ?? DateTime.UtcNow
        };

        return await repository.CreateOrUpdateAsync(plan);
    }
}