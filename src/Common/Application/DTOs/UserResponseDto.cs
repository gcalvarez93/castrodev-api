// Path: src/Common/Application/DTOs/UserResponseDto.cs
namespace Api.Common.Application.DTOs;

public sealed record UserResponseDto(
    string Id,
    string Name,
    string Email,
    string PhotoUrl,
    string Language,
    bool Notifications,
    DateTime CreatedAt
);