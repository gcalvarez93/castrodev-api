// Path: src/Modules/RecipeManager/Application/UseCases/ImportExternalRecipeHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class ImportExternalRecipeHandler(
    IRecipeRepository repository,
    ILogger<ImportExternalRecipeHandler> logger
) : IRequestHandler<ImportExternalRecipeCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        ImportExternalRecipeCommand request, CancellationToken cancellationToken)
    {
        var ext = request.ExternalRecipe;

        var steps = ext.Instructions
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select((desc, i) => new RecipeStep { Order = i + 1, Description = desc.Trim() })
            .Where(s => !string.IsNullOrWhiteSpace(s.Description))
            .ToList();

        var recipe = new Recipe
        {
            UserId      = request.UserId,
            Name        = ext.Name,
            Description = $"{ext.Area} · {ext.Category}",
            Category    = ext.Category,
            ImageUrl    = ext.ImageUrl,
            Servings    = 4,
            Difficulty  = "medium",
            Ingredients = ext.Ingredients.Select(i => new Ingredient { Name = i.Name, Amount = i.Amount, Unit = i.Unit }).ToList(),
            Steps       = steps,
            Tags        = [ext.Category, ext.Area],
            IsFavorite  = false,
            ExternalId  = ext.ExternalId,
            IsImported  = true,
            CreatedAt   = DateTime.UtcNow
        };

        var id = await repository.CreateAsync(recipe);
        logger.LogInformation("External recipe {ExternalId} imported as {Id} for user {UserId}",
            ext.ExternalId, id, request.UserId);
        return id;
    }
}