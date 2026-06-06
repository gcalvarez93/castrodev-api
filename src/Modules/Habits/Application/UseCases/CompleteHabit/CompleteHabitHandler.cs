// Path: src/Modules/Habits/Application/UseCases/CompleteHabit/CompleteHabitHandler.cs
using Api.Modules.Habits.Application.DTOs;
using Api.Modules.Habits.Domain.Entities;
using Api.Modules.Habits.Domain.Repositories;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.CompleteHabit;

public sealed class CompleteHabitHandler(
    IHabitCompletionRepository completionRepository
) : IRequestHandler<CompleteHabitCommand, HabitCompletionResponseDto>
{
    public async Task<HabitCompletionResponseDto> Handle(
        CompleteHabitCommand request,
        CancellationToken cancellationToken)
    {
        var completion = new HabitCompletion
        {
            UserId = request.UserId,
            HabitId = request.HabitId
        };

        var id = await completionRepository.CreateAsync(completion);

        return new HabitCompletionResponseDto(
            id,
            completion.HabitId,
            completion.CompletedAt
        );
    }
}