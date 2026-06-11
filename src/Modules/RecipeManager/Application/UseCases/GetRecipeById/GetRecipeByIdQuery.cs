// Path: src/Modules/RecipeManager/Application/UseCases/GetRecipeByIdQuery.cs
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record GetRecipeByIdQuery(string Id, string UserId)
    : IRequest<ErrorOr<RecipeDto>>;