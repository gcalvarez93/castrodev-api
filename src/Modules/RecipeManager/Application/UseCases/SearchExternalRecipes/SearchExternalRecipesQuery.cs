// Path: src/Modules/RecipeManager/Application/UseCases/SearchExternalRecipesQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record SearchExternalRecipesQuery(string Query)
    : IRequest<ErrorOr<List<ExternalRecipeDto>>>;