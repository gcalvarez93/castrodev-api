// Path: src/Modules/Habits/Application/UseCases/GetHabits/GetHabitsHandler.cs
using Api.Modules.Habits.Application.DTOs;
using Api.Modules.Habits.Domain.Repositories;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.GetHabits;

public sealed class GetHabitsHandler(
    IHabitRepository repository
) : IRequestHandler<GetHabitsQuery, IEnumerable<HabitResponseDto>>
{
    public async Task<IEnumerable<HabitResponseDto>> Handle(
        GetHabitsQuery request,
        CancellationToken cancellationToken)
    {
        var habits = await repository.GetAllAsync(request.UserId);

        return habits.Select(h => new HabitResponseDto(
            h.Id,
            h.Name,
            h.Icon,
            h.Color,
            h.Frequency,
            h.Streak,
            h.CreatedAt
        ));
    }
}