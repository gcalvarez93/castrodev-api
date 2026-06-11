// Path: src/Modules/RecipeManager/Application/UseCases/DeleteRecipeHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class DeleteRecipeHandler(
    IRecipeRepository repository
) : IRequestHandler<DeleteRecipeCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Recipe.NotFound", "Recipe not found.");

        await repository.DeleteAsync(request.Id, request.UserId);
        return Result.Deleted;
    }
}