// Path: src/Modules/RecipeManager/Application/UseCases/DeleteMealPlanEntryHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Domain.Repositories;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class DeleteMealPlanEntryHandler(
    IMealPlanRepository repository
) : IRequestHandler<DeleteMealPlanEntryCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteMealPlanEntryCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteEntryAsync(
            request.UserId, request.Year, request.Week,
            request.DayOfWeek, request.MealType);
        return Result.Deleted;
    }
}