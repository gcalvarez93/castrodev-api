// Path: src/Modules/Tasks/Application/DTOs/UpdateTaskDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record UpdateTaskDto(
    string Title,
    string Description,
    string Status,
    string Priority,
    DateTime? DueDate
);