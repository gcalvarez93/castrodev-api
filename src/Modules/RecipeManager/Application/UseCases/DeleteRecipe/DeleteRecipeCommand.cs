// Path: src/Modules/RecipeManager/Application/UseCases/DeleteRecipeCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record DeleteRecipeCommand(string Id, string UserId) : IRequest<ErrorOr<Deleted>>;