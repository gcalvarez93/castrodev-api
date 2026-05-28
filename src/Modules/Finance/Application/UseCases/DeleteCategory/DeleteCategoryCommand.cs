// Path: src/Modules/Finance/Application/UseCases/DeleteCategory/DeleteCategoryCommand.cs
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.DeleteCategory;

public sealed record DeleteCategoryCommand(string UserId, string Id) : IRequest;