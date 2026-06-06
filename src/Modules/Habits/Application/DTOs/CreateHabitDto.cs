// Path: src/Modules/Habits/Application/DTOs/CreateHabitDto.cs
namespace Api.Modules.Habits.Application.DTOs;

public sealed record CreateHabitDto(
    string Name,
    string Icon,
    string Color,
    string Frequency
);