// Path: src/Modules/Tasks/Application/DTOs/CreateBoardDto.cs
namespace Api.Modules.Tasks.Application.DTOs;

public sealed record CreateBoardDto(
    string Name,
    string Description,
    string Color
);