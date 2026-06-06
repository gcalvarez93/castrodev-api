// Path: src/Modules/Tasks/Application/DTOs/TaskResponseDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record TaskResponseDto(
    string Id,
    string BoardId,
    string Title,
    string Description,
    string Status,
    string Priority,
    DateTime? DueDate,
    DateTime CreatedAt
);