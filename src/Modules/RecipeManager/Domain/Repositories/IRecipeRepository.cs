// Path: src/Modules/RecipeManager/Domain/Repositories/IRecipeRepository.cs
using Api.Modules.RecipeManager.Domain.Entities;

namespace Api.Modules.RecipeManager.Domain.Repositories;

public interface IRecipeRepository
{
    Task<IEnumerable<Recipe>> GetAllAsync(string userId, string? category = null, bool? favoritesOnly = null);
    Task<Recipe?> GetByIdAsync(string id, string userId);
    Task<string> CreateAsync(Recipe recipe);
    Task UpdateAsync(Recipe recipe);
    Task DeleteAsync(string id, string userId);
    Task ToggleFavoriteAsync(string id, string userId);
}