// Path: src/Modules/Habits/Application/DTOs/HabitResponseDto.cs
namespace Api.Modules.Habits.Application.DTOs;

public sealed record HabitResponseDto(
    string Id,
    string Name,
    string Description,
    string Icon,
    string Color,
    string Frequency,
    int Streak,
    int BestStreak,
    int TotalCompleted,
    bool CompletedToday,
    DateTime CreatedAt
);