// Path: src/Modules/Tasks/Application/UseCases/DeleteBoard/DeleteBoardCommand.cs
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.DeleteBoard;

public sealed record DeleteBoardCommand(string UserId, string Id) : IRequest;