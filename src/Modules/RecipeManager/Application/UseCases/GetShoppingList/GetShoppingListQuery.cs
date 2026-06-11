// Path: src/Modules/RecipeManager/Application/UseCases/GetShoppingListQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record GetShoppingListQuery(string UserId, int Year, int Week)
    : IRequest<ErrorOr<ShoppingListDto?>>;