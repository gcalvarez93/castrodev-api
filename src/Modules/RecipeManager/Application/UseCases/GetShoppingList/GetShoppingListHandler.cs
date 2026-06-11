// Path: src/Modules/RecipeManager/Application/UseCases/GetShoppingListHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class GetShoppingListHandler(
    IShoppingListRepository repository
) : IRequestHandler<GetShoppingListQuery, ErrorOr<ShoppingListDto?>>
{
    public async Task<ErrorOr<ShoppingListDto?>> Handle(
        GetShoppingListQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetByWeekAsync(request.UserId, request.Year, request.Week);
        if (list is null) return (ShoppingListDto?)null;

        return new ShoppingListDto(
            list.Id, list.Name, list.Year, list.Week,
            list.Items.Select(i => new ShoppingListItemDto(i.Name, i.Amount, i.Unit, i.IsChecked)).ToList(),
            list.CreatedAt
        );
    }
}