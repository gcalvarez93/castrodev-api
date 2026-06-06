// Path: src/Modules/Tasks/Application/DTOs/BoardResponseDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record BoardResponseDto(
    string Id,
    string Name,
    string Color,
    DateTime CreatedAt
);