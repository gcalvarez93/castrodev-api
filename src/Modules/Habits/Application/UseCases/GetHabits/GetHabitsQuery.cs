// Path: src/Modules/Habits/Application/UseCases/GetHabits/GetHabitsQuery.cs
using Api.Modules.Habits.Application.DTOs;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.GetHabits;

public sealed record GetHabitsQuery(string UserId) : IRequest<IEnumerable<HabitResponseDto>>;