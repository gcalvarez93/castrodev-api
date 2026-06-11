// Path: src/Modules/WorkoutTracker/Application/UseCases/StartSession/StartSessionHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.StartSession;
public sealed class StartSessionHandler(IRoutineRepository routineRepo, IWorkoutSessionRepository sessionRepo, ILogger<StartSessionHandler> logger)
    : IRequestHandler<StartSessionCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(StartSessionCommand request, CancellationToken ct)
    {
        var routine = await routineRepo.GetByIdAsync(request.RoutineId, request.UserId);
        if (routine is null) return Error.NotFound("Routine.NotFound", "Routine not found.");
        var session = new WorkoutSession
        {
            UserId = request.UserId, RoutineId = routine.Id, RoutineName = routine.Name,
            Exercises = routine.Exercises.Select(e => new SessionExercise {
                ExerciseId = e.Id, Name = e.Name, IsCompleted = false,
                Sets = Enumerable.Range(1, e.Sets).Select(i => new SetLog {
                    SetNumber = i, Reps = e.Reps, WeightKg = e.WeightKg, IsCompleted = false
                }).ToList()
            }).ToList(),
            StartedAt = DateTime.UtcNow, IsCompleted = false
        };
        var id = await sessionRepo.CreateAsync(session);
        logger.LogInformation("Session {Id} started for routine {RoutineId}", id, request.RoutineId);
        return id;
    }
}