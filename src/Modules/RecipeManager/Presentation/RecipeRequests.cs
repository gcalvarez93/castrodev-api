// Path: src/Modules/RecipeManager/Presentation/RecipeRequests.cs
namespace Api.Modules.RecipeManager.Presentation;

public sealed record CreateRecipeRequest(
    string Name,
    string Description,
    string Category,
    string ImageUrl,
    int PrepTimeMinutes,
    int CookTimeMinutes,
    int Servings,
    string Difficulty,
    List<IngredientRequest> Ingredients,
    List<RecipeStepRequest> Steps,
    List<string> Tags
);

public sealed record UpdateRecipeRequest(
    string Name,
    string Description,
    string Category,
    string ImageUrl,
    int PrepTimeMinutes,
    int CookTimeMinutes,
    int Servings,
    string Difficulty,
    List<IngredientRequest> Ingredients,
    List<RecipeStepRequest> Steps,
    List<string> Tags
);

public sealed record IngredientRequest(string Name, string Amount, string Unit);
public sealed record RecipeStepRequest(int Order, string Description);

public sealed record ImportExternalRecipeRequest(
    string ExternalId,
    string Name,
    string Category,
    string Area,
    string Instructions,
    string ImageUrl,
    List<IngredientRequest> Ingredients,
    string? YoutubeUrl
);

public sealed record UpsertMealPlanEntryRequest(
    string DayOfWeek,
    string MealType,
    string RecipeId,
    string RecipeName
);