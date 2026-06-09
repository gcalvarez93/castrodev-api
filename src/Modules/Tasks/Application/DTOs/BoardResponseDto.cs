// Path: src/Modules/Tasks/Application/DTOs/BoardResponseDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record BoardResponseDto(
    string Id,
    string Name,
    string Description,
    string Color,
    int TaskCount,
    DateTime CreatedAt
);