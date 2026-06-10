// Path: src/Modules/Tasks/Application/UseCases/GetTasks/GetTasksHandler.cs
using Api.Modules.Tasks.Application.DTOs;
using Api.Modules.Tasks.Domain.Repositories;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.GetTasks;

public sealed class GetTasksHandler(
    ITaskRepository repository
) : IRequestHandler<GetTasksQuery, IEnumerable<TaskResponseDto>>
{
    public async Task<IEnumerable<TaskResponseDto>> Handle(
        GetTasksQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = request.BoardId is not null
            ? await repository.GetByBoardAsync(request.BoardId, request.UserId)
            : await repository.GetAllAsync(request.UserId);

        return tasks.Select(t => new TaskResponseDto(
            t.Id,
            t.BoardId,
            t.Title,
            t.Description,
            t.Status,
            t.Priority,
            t.DueDate,
            t.Labels,
            t.CreatedAt
        ));
    }
}