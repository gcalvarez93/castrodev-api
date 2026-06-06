// Path: src/Common/Application/DTOs/UpdateUserDto.cs
namespace Api.Common.Application.DTOs;

public sealed record UpdateUserDto(
    string Name,
    string PhotoUrl,
    string Language,
    bool Notifications
);