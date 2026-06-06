// Path: src/Modules/Tasks/Application/DTOs/CreateLabelDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record CreateLabelDto(
    string Name,
    string Color
);