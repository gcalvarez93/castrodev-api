// Path: src/Modules/Tasks/Application/DTOs/CreateTaskDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record CreateTaskDto(
    string BoardId,
    string Title,
    string Description,
    string Priority,
    DateTime? DueDate
);