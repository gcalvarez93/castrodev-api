// Path: src/Modules/RecipeManager/Application/UseCases/ToggleShoppingItemHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class ToggleShoppingItemHandler(
    IShoppingListRepository repository
) : IRequestHandler<ToggleShoppingItemCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(
        ToggleShoppingItemCommand request, CancellationToken cancellationToken)
    {
        await repository.ToggleItemAsync(request.Id, request.UserId, request.ItemName);
        return Result.Updated;
    }
}