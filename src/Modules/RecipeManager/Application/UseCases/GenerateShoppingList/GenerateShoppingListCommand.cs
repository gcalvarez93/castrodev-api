// Path: src/Modules/RecipeManager/Application/UseCases/GenerateShoppingListCommand.cs
using ErrorOr;
using MediatR;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed record GenerateShoppingListCommand(string UserId, int Year, int Week)
    : IRequest<ErrorOr<string>>;