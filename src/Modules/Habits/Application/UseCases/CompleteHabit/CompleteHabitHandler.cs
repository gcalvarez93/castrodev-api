// Path: src/Modules/Habits/Application/UseCases/CompleteHabit/CompleteHabitHandler.cs
using Api.Modules.Habits.Application.DTOs;
using Api.Modules.Habits.Domain.Entities;
using Api.Modules.Habits.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.CompleteHabit;

public sealed class CompleteHabitHandler(
    IHabitRepository habitRepository,
    IHabitCompletionRepository completionRepository
) : IRequestHandler<CompleteHabitCommand, ErrorOr<HabitResponseDto>>
{
    public async Task<ErrorOr<HabitResponseDto>> Handle(
        CompleteHabitCommand request,
        CancellationToken cancellationToken)
    {
        var habit = await habitRepository.GetByIdAsync(request.HabitId, request.UserId);
        if (habit is null) return Error.NotFound("Habit.NotFound", "Habit not found.");

        // Registrar completion
        var completion = new HabitCompletion
        {
            UserId = request.UserId,
            HabitId = request.HabitId
        };
        await completionRepository.CreateAsync(completion);

        // Calcular nueva racha y totales
        var newStreak = habit.Streak + 1;
        var newBestStreak = Math.Max(habit.BestStreak, newStreak);
        var newTotalCompleted = habit.TotalCompleted + 1;

        // Actualizar hábito
        var updatedHabit = new Habit
        {
            Id = habit.Id,
            UserId = habit.UserId,
            Name = habit.Name,
            Description = habit.Description,
            Icon = habit.Icon,
            Color = habit.Color,
            Frequency = habit.Frequency,
            Streak = newStreak,
            BestStreak = newBestStreak,
            TotalCompleted = newTotalCompleted,
            CreatedAt = habit.CreatedAt
        };

        await habitRepository.UpdateAsync(updatedHabit);

        return new HabitResponseDto(
            updatedHabit.Id,
            updatedHabit.Name,
            updatedHabit.Description,
            updatedHabit.Icon,
            updatedHabit.Color,
            updatedHabit.Frequency,
            updatedHabit.Streak,
            updatedHabit.BestStreak,
            updatedHabit.TotalCompleted,
            true,
            updatedHabit.CreatedAt
        );
    }
}