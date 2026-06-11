// Path: src/Modules/WorkoutTracker/Application/UseCases/GetExerciseProgress/GetExerciseProgressHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetExerciseProgress;
public sealed class GetExerciseProgressHandler(IWorkoutSessionRepository repository)
    : IRequestHandler<GetExerciseProgressQuery, ErrorOr<ExerciseProgressDto>>
{
    public async Task<ErrorOr<ExerciseProgressDto>> Handle(GetExerciseProgressQuery request, CancellationToken ct)
    {
        var sessions = await repository.GetAllAsync(request.UserId);
        var points = sessions
            .Where(s => s.IsCompleted)
            .SelectMany(s => s.Exercises
                .Where(e => e.Name.Equals(request.ExerciseName, StringComparison.OrdinalIgnoreCase))
                .Select(e => new ProgressPointDto(
                    s.CompletedAt!.Value,
                    e.Sets.Where(set => set.IsCompleted).Max(set => set.WeightKg),
                    e.Sets.Where(set => set.IsCompleted).Sum(set => set.Reps)
                )))
            .OrderBy(p => p.Date)
            .ToList();
        return new ExerciseProgressDto(request.ExerciseName, points);
    }
}