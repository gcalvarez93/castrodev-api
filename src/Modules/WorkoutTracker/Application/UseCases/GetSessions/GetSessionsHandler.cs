// Path: src/Modules/WorkoutTracker/Application/UseCases/GetSessions/GetSessionsHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetSessions;
public sealed class GetSessionsHandler(IWorkoutSessionRepository repository)
    : IRequestHandler<GetSessionsQuery, ErrorOr<List<WorkoutSessionDto>>>
{
    public async Task<ErrorOr<List<WorkoutSessionDto>>> Handle(GetSessionsQuery request, CancellationToken ct)
    {
        var sessions = await repository.GetAllAsync(request.UserId, request.RoutineId);
        return sessions.Select(s => new WorkoutSessionDto(
            s.Id, s.RoutineId, s.RoutineName,
            s.Exercises.Select(e => new SessionExerciseDto(e.ExerciseId, e.Name,
                e.Sets.Select(set => new SetLogDto(set.SetNumber, set.Reps, set.WeightKg, set.IsCompleted)).ToList(),
                e.IsCompleted)).ToList(),
            s.StartedAt, s.CompletedAt, s.IsCompleted, s.DurationMinutes, s.TotalVolumeKg
        )).ToList();
    }
}