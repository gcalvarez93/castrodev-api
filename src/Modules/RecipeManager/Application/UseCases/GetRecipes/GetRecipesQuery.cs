// Path: src/Modules/RecipeManager/Application/UseCases/GetRecipesQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record GetRecipesQuery(string UserId, string? Category, bool? FavoritesOnly)
    : IRequest<ErrorOr<List<RecipeDto>>>;