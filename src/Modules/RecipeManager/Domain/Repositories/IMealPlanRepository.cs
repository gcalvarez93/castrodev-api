// Path: src/Modules/RecipeManager/Domain/Repositories/IMealPlanRepository.cs
using Api.Modules.RecipeManager.Domain.Entities;

namespace Api.Modules.RecipeManager.Domain.Repositories;

public interface IMealPlanRepository
{
    Task<MealPlan?> GetByWeekAsync(string userId, int year, int week);
    Task<string> CreateOrUpdateAsync(MealPlan mealPlan);
    Task DeleteEntryAsync(string userId, int year, int week, string dayOfWeek, string mealType);
}