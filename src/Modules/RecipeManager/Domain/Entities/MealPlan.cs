// Path: src/Modules/RecipeManager/Domain/Entities/MealPlan.cs
namespace Api.Modules.RecipeManager.Domain.Entities;

public sealed class MealPlan
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public int Year { get; init; }
    public int Week { get; init; }
    public List<MealPlanEntry> Entries { get; init; } = [];
    public DateTime CreatedAt { get; init; }
}

public sealed class MealPlanEntry
{
    public string DayOfWeek { get; init; } = string.Empty;
    public string MealType { get; init; } = string.Empty;
    public string RecipeId { get; init; } = string.Empty;
    public string RecipeName { get; init; } = string.Empty;
}