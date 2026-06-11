// Path: src/Modules/WorkoutTracker/Application/UseCases/GetSessionById/GetSessionByIdHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetSessionById;
public sealed class GetSessionByIdHandler(IWorkoutSessionRepository repository)
    : IRequestHandler<GetSessionByIdQuery, ErrorOr<WorkoutSessionDto>>
{
    public async Task<ErrorOr<WorkoutSessionDto>> Handle(GetSessionByIdQuery request, CancellationToken ct)
    {
        var s = await repository.GetByIdAsync(request.Id, request.UserId);
        if (s is null) return Error.NotFound("Session.NotFound", "Session not found.");
        return new WorkoutSessionDto(
            s.Id, s.RoutineId, s.RoutineName,
            s.Exercises.Select(e => new SessionExerciseDto(e.ExerciseId, e.Name,
                e.Sets.Select(set => new SetLogDto(set.SetNumber, set.Reps, set.WeightKg, set.IsCompleted)).ToList(),
                e.IsCompleted)).ToList(),
            s.StartedAt, s.CompletedAt, s.IsCompleted, s.DurationMinutes, s.TotalVolumeKg);
    }
}