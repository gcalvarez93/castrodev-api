// Path: src/Modules/Tasks/Application/UseCases/GetTasks/GetTasksQuery.cs
using Api.Modules.Tasks.Application.DTOs;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.GetTasks;

public sealed record GetTasksQuery(string UserId, string? BoardId) : IRequest<IEnumerable<TaskResponseDto>>;