// Path: src/Modules/RecipeManager/Application/UseCases/GetMealPlanQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record GetMealPlanQuery(string UserId, int Year, int Week)
    : IRequest<ErrorOr<MealPlanDto?>>;