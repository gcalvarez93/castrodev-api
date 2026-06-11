// Path: src/Modules/WorkoutTracker/Application/UseCases/CompleteSession/CompleteSessionHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.CompleteSession;
public sealed class CompleteSessionHandler(IWorkoutSessionRepository repository, ILogger<CompleteSessionHandler> logger)
    : IRequestHandler<CompleteSessionCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(CompleteSessionCommand request, CancellationToken ct)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Session.NotFound", "Session not found.");

        var completedAt = DateTime.UtcNow;
        var duration    = (int)(completedAt - existing.StartedAt).TotalMinutes;

        var exercises = request.Exercises.Select(e => new SessionExercise {
            ExerciseId  = e.ExerciseId,
            Name        = e.Name,
            IsCompleted = e.IsCompleted,
            Sets        = e.Sets.Select(s => new SetLog {
                SetNumber   = s.SetNumber,
                Reps        = s.Reps,
                WeightKg    = s.WeightKg,
                IsCompleted = s.IsCompleted
            }).ToList()
        }).ToList();

        var totalVolume = exercises.Sum(e => e.Sets.Where(s => s.IsCompleted).Sum(s => s.Reps * s.WeightKg));

        var updated = new WorkoutSession
        {
            Id              = existing.Id,
            UserId          = existing.UserId,
            RoutineId       = existing.RoutineId,
            RoutineName     = existing.RoutineName,
            Exercises       = exercises,
            StartedAt       = existing.StartedAt,
            CompletedAt     = completedAt,
            IsCompleted     = true,
            DurationMinutes = duration,
            TotalVolumeKg   = totalVolume
        };

        await repository.UpdateAsync(updated);
        logger.LogInformation("Session {Id} completed for user {UserId}", request.Id, request.UserId);
        return Result.Updated;
    }
}