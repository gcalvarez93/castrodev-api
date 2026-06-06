// Path: src/Modules/Tasks/Application/DTOs/LabelResponseDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record LabelResponseDto(
    string Id,
    string Name,
    string Color,
    DateTime CreatedAt
);