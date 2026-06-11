// Path: src/Modules/RecipeManager/Application/UseCases/GetRecipesHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class GetRecipesHandler(
    IRecipeRepository repository
) : IRequestHandler<GetRecipesQuery, ErrorOr<List<RecipeDto>>>
{
    public async Task<ErrorOr<List<RecipeDto>>> Handle(
        GetRecipesQuery request, CancellationToken cancellationToken)
    {
        var recipes = await repository.GetAllAsync(request.UserId, request.Category, request.FavoritesOnly);
        return recipes.Select(MapToDto).ToList();
    }

    private static RecipeDto MapToDto(Domain.Entities.Recipe r) => new(
        r.Id, r.Name, r.Description, r.Category, r.ImageUrl,
        r.PrepTimeMinutes, r.CookTimeMinutes, r.Servings, r.Difficulty,
        r.Ingredients.Select(i => new IngredientDto(i.Name, i.Amount, i.Unit)).ToList(),
        r.Steps.Select(s => new RecipeStepDto(s.Order, s.Description)).ToList(),
        r.Tags, r.IsFavorite, r.ExternalId, r.IsImported, r.CreatedAt
    );
}