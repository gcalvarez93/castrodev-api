// Path: src/Modules/Finance/Application/DTOs/CategoryResponseDto.cs
namespace Api.Modules.Finance.Application.DTOs;

public sealed record CategoryResponseDto(
    string Id,
    string Name,
    string Color,
    string Icon,
    DateTime CreatedAt
);