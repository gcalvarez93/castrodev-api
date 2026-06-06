// Path: src/Modules/Tasks/Application/UseCases/CreateTask/CreateTaskCommand.cs
using Api.Modules.Tasks.Application.DTOs;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.CreateTask;

public sealed record CreateTaskCommand(
    string UserId,
    CreateTaskDto Dto
) : IRequest<TaskResponseDto>;