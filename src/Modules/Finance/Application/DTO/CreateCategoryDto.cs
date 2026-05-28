// Path: src/Modules/Finance/Application/DTOs/CreateCategoryDto.cs
namespace Api.Modules.Finance.Application.DTOs;

public sealed record CreateCategoryDto(
    string Name,
    string Color,
    string Icon
);