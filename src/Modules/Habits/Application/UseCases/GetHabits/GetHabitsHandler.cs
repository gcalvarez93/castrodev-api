// Path: src/Modules/Habits/Application/UseCases/GetHabits/GetHabitsHandler.cs
using Api.Modules.Habits.Application.DTOs;
using Api.Modules.Habits.Domain.Repositories;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.GetHabits;

public sealed class GetHabitsHandler(
    IHabitRepository habitRepository,
    IHabitCompletionRepository completionRepository
) : IRequestHandler<GetHabitsQuery, IEnumerable<HabitResponseDto>>
{
    public async Task<IEnumerable<HabitResponseDto>> Handle(
        GetHabitsQuery request,
        CancellationToken cancellationToken)
    {
        var habits = await habitRepository.GetAllAsync(request.UserId);
        var todayCompletions = await completionRepository.GetByDateAsync(
            request.UserId, DateTime.UtcNow);

        var completedTodayIds = todayCompletions.Select(c => c.HabitId).ToHashSet();

        return habits.Select(h => new HabitResponseDto(
            h.Id,
            h.Name,
            h.Description,
            h.Icon,
            h.Color,
            h.Frequency,
            h.Streak,
            h.BestStreak,
            h.TotalCompleted,
            completedTodayIds.Contains(h.Id),
            h.CreatedAt
        ));
    }
}