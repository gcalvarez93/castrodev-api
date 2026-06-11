// Path: src/Modules/WorkoutTracker/Application/UseCases/GetExercises/GetExercisesHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetExercises;
public sealed class GetExercisesHandler(IRoutineRepository repository)
    : IRequestHandler<GetExercisesQuery, ErrorOr<List<ExerciseDto>>>
{
    public async Task<ErrorOr<List<ExerciseDto>>> Handle(GetExercisesQuery request, CancellationToken ct)
    {
        var routine = await repository.GetByIdAsync(request.RoutineId, request.UserId);
        if (routine is null) return Error.NotFound("Routine.NotFound", "Routine not found.");
        return routine.Exercises.Select(e => new ExerciseDto(e.Id, e.Name, e.MuscleGroup, e.Equipment,
            e.Sets, e.Reps, e.WeightKg, e.RestSeconds, e.Notes, e.ExternalId, e.IsImported)).ToList();
    }
}