// Path: src/Modules/Habits/Application/UseCases/CompleteHabit/CompleteHabitCommand.cs
using Api.Modules.Habits.Application.DTOs;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.CompleteHabit;

public sealed record CompleteHabitCommand(
    string UserId,
    string HabitId
) : IRequest<HabitResponseDto>;