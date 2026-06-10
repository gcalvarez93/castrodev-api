// Path: src/Modules/Tasks/Application/UseCases/UpdateTask/UpdateTaskHandler.cs
using Api.Modules.Tasks.Application.DTOs;
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.UpdateTask;

public sealed class UpdateTaskHandler(
    ITaskRepository repository
) : IRequestHandler<UpdateTaskCommand, TaskResponseDto>
{
    public async Task<TaskResponseDto> Handle(
        UpdateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) throw new KeyNotFoundException();

        var task = new TaskItem
        {
            Id = request.Id,
            UserId = request.UserId,
            BoardId = existing.BoardId,
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            Status = request.Dto.Status,
            Priority = request.Dto.Priority,
            DueDate = request.Dto.DueDate,
            Labels = request.Dto.Labels,
            CreatedAt = existing.CreatedAt
        };

        await repository.UpdateAsync(task);

        return new TaskResponseDto(
            task.Id,
            task.BoardId,
            task.Title,
            task.Description,
            task.Status,
            task.Priority,
            task.DueDate,
            task.Labels,
            task.CreatedAt
        );
    }
}