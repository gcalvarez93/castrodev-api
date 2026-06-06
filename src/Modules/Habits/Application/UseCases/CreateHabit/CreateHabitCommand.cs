// Path: src/Modules/Habits/Application/UseCases/CreateHabit/CreateHabitCommand.cs
using Api.Modules.Habits.Application.DTOs;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.CreateHabit;

public sealed record CreateHabitCommand(
    string UserId,
    CreateHabitDto Dto
) : IRequest<HabitResponseDto>;