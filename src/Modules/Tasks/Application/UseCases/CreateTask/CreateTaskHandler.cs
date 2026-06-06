// Path: src/Modules/Tasks/Application/UseCases/CreateTask/CreateTaskHandler.cs
using Api.Modules.Tasks.Application.DTOs;
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.CreateTask;

public sealed class CreateTaskHandler(
    ITaskRepository repository
) : IRequestHandler<CreateTaskCommand, TaskResponseDto>
{
    public async Task<TaskResponseDto> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = new TaskItem
        {
            UserId = request.UserId,
            BoardId = request.Dto.BoardId,
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            Priority = request.Dto.Priority,
            DueDate = request.Dto.DueDate
        };

        var id = await repository.CreateAsync(task);

        return new TaskResponseDto(
            id,
            task.BoardId,
            task.Title,
            task.Description,
            task.Status,
            task.Priority,
            task.DueDate,
            task.CreatedAt
        );
    }
}