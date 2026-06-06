// Path: src/Modules/Tasks/Application/UseCases/DeleteTask/DeleteTaskCommand.cs
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.DeleteTask;

public sealed record DeleteTaskCommand(string UserId, string Id) : IRequest;