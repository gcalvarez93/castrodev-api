// Path: src/Modules/RecipeManager/Application/UseCases/ToggleFavoriteCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record ToggleFavoriteCommand(string Id, string UserId) : IRequest<ErrorOr<Updated>>;