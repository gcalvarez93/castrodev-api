// Path: src/Modules/RecipeManager/Application/UseCases/UpdateRecipeHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class UpdateRecipeHandler(
    IRecipeRepository repository
) : IRequestHandler<UpdateRecipeCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(
        UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Recipe.NotFound", "Recipe not found.");

        var updated = new Recipe
        {
            Id              = request.Id,
            UserId          = request.UserId,
            Name            = request.Name,
            Description     = request.Description,
            Category        = request.Category,
            ImageUrl        = request.ImageUrl,
            PrepTimeMinutes = request.PrepTimeMinutes,
            CookTimeMinutes = request.CookTimeMinutes,
            Servings        = request.Servings,
            Difficulty      = request.Difficulty,
            Ingredients     = request.Ingredients.Select(i => new Ingredient { Name = i.Name, Amount = i.Amount, Unit = i.Unit }).ToList(),
            Steps           = request.Steps.Select(s => new RecipeStep { Order = s.Order, Description = s.Description }).ToList(),
            Tags            = request.Tags,
            IsFavorite      = existing.IsFavorite,
            ExternalId      = existing.ExternalId,
            IsImported      = existing.IsImported,
            CreatedAt       = existing.CreatedAt
        };

        await repository.UpdateAsync(updated);
        return Result.Updated;
    }
}