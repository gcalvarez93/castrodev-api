// Path: src/Modules/WorkoutTracker/Application/UseCases/GetRoutines/GetRoutinesHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetRoutines;
public sealed class GetRoutinesHandler(IRoutineRepository repository)
    : IRequestHandler<GetRoutinesQuery, ErrorOr<List<RoutineDto>>>
{
    public async Task<ErrorOr<List<RoutineDto>>> Handle(GetRoutinesQuery request, CancellationToken ct)
    {
        var routines = await repository.GetAllAsync(request.UserId);
        return routines.Select(MapToDto).ToList();
    }
    private static RoutineDto MapToDto(Domain.Entities.Routine r) => new(
        r.Id, r.Name, r.Description, r.Category, r.DaysOfWeek,
        r.Exercises.Select(e => new ExerciseDto(e.Id, e.Name, e.MuscleGroup, e.Equipment,
            e.Sets, e.Reps, e.WeightKg, e.RestSeconds, e.Notes, e.ExternalId, e.IsImported)).ToList(),
        r.CreatedAt);
}