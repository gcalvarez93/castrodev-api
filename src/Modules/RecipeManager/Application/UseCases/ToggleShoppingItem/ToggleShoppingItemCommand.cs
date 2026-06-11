// Path: src/Modules/RecipeManager/Application/UseCases/ToggleShoppingItemCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record ToggleShoppingItemCommand(string Id, string UserId, string ItemName)
    : IRequest<ErrorOr<Updated>>;