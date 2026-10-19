// Path: src/Modules/Habits/Application/UseCases/CompleteHabit/CompleteHabitCommand.cs
using Api.Modules.Habits.Application.DTOs;
using ErrorOr;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.CompleteHabit;

public sealed record CompleteHabitCommand(
    string UserId,
    string HabitId
) : IRequest<ErrorOr<HabitResponseDto>>;