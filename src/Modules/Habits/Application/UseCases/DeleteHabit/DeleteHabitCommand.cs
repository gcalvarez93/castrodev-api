// Path: src/Modules/Habits/Application/UseCases/DeleteHabit/DeleteHabitCommand.cs
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.DeleteHabit;

public sealed record DeleteHabitCommand(string UserId, string Id) : IRequest;