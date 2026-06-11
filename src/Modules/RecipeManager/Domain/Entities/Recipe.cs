// Path: src/Modules/RecipeManager/Domain/Entities/Recipe.cs
namespace Api.Modules.RecipeManager.Domain.Entities;

public sealed class Recipe
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public int PrepTimeMinutes { get; init; }
    public int CookTimeMinutes { get; init; }
    public int Servings { get; init; }
    public string Difficulty { get; init; } = "medium";
    public List<Ingredient> Ingredients { get; init; } = [];
    public List<RecipeStep> Steps { get; init; } = [];
    public List<string> Tags { get; init; } = [];
    public bool IsFavorite { get; init; }
    public string? ExternalId { get; init; }
    public bool IsImported { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed class Ingredient
{
    public string Name { get; init; } = string.Empty;
    public string Amount { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
}

public sealed class RecipeStep
{
    public int Order { get; init; }
    public string Description { get; init; } = string.Empty;
}