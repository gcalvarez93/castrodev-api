// Path: src/Modules/RecipeManager/Domain/Entities/ShoppingList.cs
namespace Api.Modules.RecipeManager.Domain.Entities;

public sealed class ShoppingList
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int Year { get; init; }
    public int Week { get; init; }
    public List<ShoppingListItem> Items { get; init; } = [];
    public DateTime CreatedAt { get; init; }
}

public sealed class ShoppingListItem
{
    public string Name { get; init; } = string.Empty;
    public string Amount { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public bool IsChecked { get; init; }
}