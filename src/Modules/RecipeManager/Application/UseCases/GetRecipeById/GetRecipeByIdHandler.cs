// Path: src/Modules/RecipeManager/Application/UseCases/GetRecipeByIdHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class GetRecipeByIdHandler(
    IRecipeRepository repository
) : IRequestHandler<GetRecipeByIdQuery, ErrorOr<RecipeDto>>
{
    public async Task<ErrorOr<RecipeDto>> Handle(
        GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        var r = await repository.GetByIdAsync(request.Id, request.UserId);
        if (r is null) return Error.NotFound("Recipe.NotFound", "Recipe not found.");

        return new RecipeDto(
            r.Id, r.Name, r.Description, r.Category, r.ImageUrl,
            r.PrepTimeMinutes, r.CookTimeMinutes, r.Servings, r.Difficulty,
            r.Ingredients.Select(i => new IngredientDto(i.Name, i.Amount, i.Unit)).ToList(),
            r.Steps.Select(s => new RecipeStepDto(s.Order, s.Description)).ToList(),
            r.Tags, r.IsFavorite, r.ExternalId, r.IsImported, r.CreatedAt
        );
    }
}