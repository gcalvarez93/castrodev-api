// Path: src/Modules/RecipeManager/Application/DTOs/RecipeDto.cs
namespace Api.Modules.RecipeManager.Application.DTOs;

public sealed record RecipeDto(
    string Id,
    string Name,
    string Description,
    string Category,
    string ImageUrl,
    int PrepTimeMinutes,
    int CookTimeMinutes,
    int Servings,
    string Difficulty,
    List<IngredientDto> Ingredients,
    List<RecipeStepDto> Steps,
    List<string> Tags,
    bool IsFavorite,
    string? ExternalId,
    bool IsImported,
    DateTime CreatedAt
);

public sealed record IngredientDto(string Name, string Amount, string Unit);
public sealed record RecipeStepDto(int Order, string Description);