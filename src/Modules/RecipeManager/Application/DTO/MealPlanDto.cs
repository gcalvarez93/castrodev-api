// Path: src/Modules/RecipeManager/Application/DTOs/MealPlanDto.cs
namespace Api.Modules.RecipeManager.Application.DTOs;

public sealed record MealPlanDto(
    string Id,
    int Year,
    int Week,
    List<MealPlanEntryDto> Entries,
    DateTime CreatedAt
);

public sealed record MealPlanEntryDto(
    string DayOfWeek,
    string MealType,
    string RecipeId,
    string RecipeName
);