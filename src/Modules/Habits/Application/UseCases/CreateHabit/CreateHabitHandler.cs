// Path: src/Modules/Habits/Application/UseCases/CreateHabit/CreateHabitHandler.cs
using Api.Modules.Habits.Application.DTOs;
using Api.Modules.Habits.Domain.Entities;
using Api.Modules.Habits.Domain.Repositories;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.CreateHabit;

public sealed class CreateHabitHandler(
    IHabitRepository repository
) : IRequestHandler<CreateHabitCommand, HabitResponseDto>
{
    public async Task<HabitResponseDto> Handle(
        CreateHabitCommand request,
        CancellationToken cancellationToken)
    {
        var habit = new Habit
        {
            UserId = request.UserId,
            Name = request.Dto.Name,
            Icon = request.Dto.Icon,
            Color = request.Dto.Color,
            Frequency = request.Dto.Frequency
        };

        var id = await repository.CreateAsync(habit);

        return new HabitResponseDto(
            id,
            habit.Name,
            habit.Icon,
            habit.Color,
            habit.Frequency,
            habit.Streak,
            habit.CreatedAt
        );
    }
}