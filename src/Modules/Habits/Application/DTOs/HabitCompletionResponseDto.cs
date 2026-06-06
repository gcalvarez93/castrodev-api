// Path: src/Modules/Habits/Application/DTOs/HabitCompletionResponseDto.cs
namespace Api.Modules.Habits.Application.DTOs;

public sealed record HabitCompletionResponseDto(
    string Id,
    string HabitId,
    DateTime CompletedAt
);