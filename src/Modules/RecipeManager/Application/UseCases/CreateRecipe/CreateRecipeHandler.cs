// Path: src/Modules/RecipeManager/Application/UseCases/CreateRecipeHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class CreateRecipeHandler(
    IRecipeRepository repository,
    ILogger<CreateRecipeHandler> logger
) : IRequestHandler<CreateRecipeCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation("Recipe.InvalidName", "Name cannot be empty.");

        var recipe = new Recipe
        {
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
            IsFavorite      = false,
            IsImported      = false,
            CreatedAt       = DateTime.UtcNow
        };

        var id = await repository.CreateAsync(recipe);
        logger.LogInformation("Recipe {Id} created for user {UserId}", id, request.UserId);
        return id;
    }
}