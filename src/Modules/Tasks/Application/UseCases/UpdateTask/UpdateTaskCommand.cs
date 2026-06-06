// Path: src/Modules/Tasks/Application/UseCases/UpdateTask/UpdateTaskCommand.cs
using Api.Modules.Tasks.Application.DTOs;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.UpdateTask;

public sealed record UpdateTaskCommand(
    string UserId,
    string Id,
    UpdateTaskDto Dto
) : IRequest<TaskResponseDto>;