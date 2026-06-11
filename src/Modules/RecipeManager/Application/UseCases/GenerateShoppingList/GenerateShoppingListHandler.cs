// Path: src/Modules/RecipeManager/Application/UseCases/GenerateShoppingListHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class GenerateShoppingListHandler(
    IMealPlanRepository mealPlanRepository,
    IRecipeRepository recipeRepository,
    IShoppingListRepository shoppingListRepository,
    ILogger<GenerateShoppingListHandler> logger
) : IRequestHandler<GenerateShoppingListCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        GenerateShoppingListCommand request, CancellationToken cancellationToken)
    {
        var plan = await mealPlanRepository.GetByWeekAsync(request.UserId, request.Year, request.Week);
        if (plan is null || plan.Entries.Count == 0)
            return Error.NotFound("MealPlan.NotFound", "No meal plan found for this week.");

        // Recopilar todos los ingredientes de todas las recetas del plan
        var allIngredients = new List<ShoppingListItem>();
        foreach (var entry in plan.Entries)
        {
            var recipe = await recipeRepository.GetByIdAsync(entry.RecipeId, request.UserId);
            if (recipe is null) continue;

            foreach (var ingredient in recipe.Ingredients)
            {
                allIngredients.Add(new ShoppingListItem
                {
                    Name      = ingredient.Name,
                    Amount    = ingredient.Amount,
                    Unit      = ingredient.Unit,
                    IsChecked = false
                });
            }
        }

        // Agrupar ingredientes duplicados
        var grouped = allIngredients
            .GroupBy(i => i.Name.ToLower().Trim())
            .Select(g => new ShoppingListItem
            {
                Name      = g.First().Name,
                Amount    = string.Join(" + ", g.Select(i => i.Amount).Where(a => !string.IsNullOrWhiteSpace(a))),
                Unit      = g.First().Unit,
                IsChecked = false
            })
            .OrderBy(i => i.Name)
            .ToList();

        var shoppingList = new ShoppingList
        {
            UserId    = request.UserId,
            Name      = $"Week {request.Week} - {request.Year}",
            Year      = request.Year,
            Week      = request.Week,
            Items     = grouped,
            CreatedAt = DateTime.UtcNow
        };

        var id = await shoppingListRepository.CreateOrUpdateAsync(shoppingList);
        logger.LogInformation("Shopping list generated for user {UserId} week {Week}/{Year}", request.UserId, request.Week, request.Year);
        return id;
    }
}