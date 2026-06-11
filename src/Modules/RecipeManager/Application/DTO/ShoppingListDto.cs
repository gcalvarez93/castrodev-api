// Path: src/Modules/RecipeManager/Application/DTOs/ShoppingListDto.cs
namespace Api.Modules.RecipeManager.Application.DTOs;

public sealed record ShoppingListDto(
    string Id,
    string Name,
    int Year,
    int Week,
    List<ShoppingListItemDto> Items,
    DateTime CreatedAt
);

public sealed record ShoppingListItemDto(
    string Name,
    string Amount,
    string Unit,
    bool IsChecked
);