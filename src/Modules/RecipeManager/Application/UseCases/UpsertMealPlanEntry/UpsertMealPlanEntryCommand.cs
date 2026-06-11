// Path: src/Modules/RecipeManager/Application/UseCases/UpsertMealPlanEntryCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record UpsertMealPlanEntryCommand(
    string UserId,
    int Year,
    int Week,
    string DayOfWeek,
    string MealType,
    string RecipeId,
    string RecipeName
) : IRequest<ErrorOr<string>>;