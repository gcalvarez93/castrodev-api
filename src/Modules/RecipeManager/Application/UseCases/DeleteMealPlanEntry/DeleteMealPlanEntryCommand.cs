// Path: src/Modules/RecipeManager/Application/UseCases/DeleteMealPlanEntryCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record DeleteMealPlanEntryCommand(
    string UserId, int Year, int Week, string DayOfWeek, string MealType
) : IRequest<ErrorOr<Deleted>>;