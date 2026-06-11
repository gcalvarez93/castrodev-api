// Path: src/Modules/RecipeManager/Domain/Repositories/IShoppingListRepository.cs
using Api.Modules.RecipeManager.Domain.Entities;

namespace Api.Modules.RecipeManager.Domain.Repositories;

public interface IShoppingListRepository
{
    Task<ShoppingList?> GetByWeekAsync(string userId, int year, int week);
    Task<string> CreateOrUpdateAsync(ShoppingList shoppingList);
    Task ToggleItemAsync(string id, string userId, string itemName);
}