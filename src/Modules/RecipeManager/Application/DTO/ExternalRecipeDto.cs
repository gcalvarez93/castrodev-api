// Path: src/Modules/RecipeManager/Application/DTOs/ExternalRecipeDto.cs
namespace Api.Modules.RecipeManager.Application.DTOs;

public sealed record ExternalRecipeDto(
    string ExternalId,
    string Name,
    string Category,
    string Area,
    string Instructions,
    string ImageUrl,
    List<IngredientDto> Ingredients,
    string? YoutubeUrl
);