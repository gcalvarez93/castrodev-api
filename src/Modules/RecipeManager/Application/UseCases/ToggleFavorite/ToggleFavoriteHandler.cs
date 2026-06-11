// Path: src/Modules/RecipeManager/Application/UseCases/ToggleFavoriteHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class ToggleFavoriteHandler(
    IRecipeRepository repository
) : IRequestHandler<ToggleFavoriteCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(
        ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Recipe.NotFound", "Recipe not found.");

        await repository.ToggleFavoriteAsync(request.Id, request.UserId);
        return Result.Updated;
    }
}