// Path: src/Modules/RecipeManager/Application/UseCases/ImportExternalRecipeCommand.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record ImportExternalRecipeCommand(string UserId, ExternalRecipeDto ExternalRecipe)
    : IRequest<ErrorOr<string>>;