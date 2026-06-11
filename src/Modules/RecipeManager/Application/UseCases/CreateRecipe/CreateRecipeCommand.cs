// Path: src/Modules/RecipeManager/Application/UseCases/CreateRecipeCommand.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record CreateRecipeCommand(
    string UserId,
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
    List<string> Tags
) : IRequest<ErrorOr<string>>;