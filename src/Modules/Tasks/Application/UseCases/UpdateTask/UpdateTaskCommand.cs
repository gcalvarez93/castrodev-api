// Path: src/Modules/Tasks/Application/UseCases/UpdateTask/UpdateTaskCommand.cs
using Api.Modules.Tasks.Application.DTOs;
using ErrorOr;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.UpdateTask;

public sealed record UpdateTaskCommand(
    string UserId,
    string Id,
    UpdateTaskDto Dto
) : IRequest<ErrorOr<TaskResponseDto>>;