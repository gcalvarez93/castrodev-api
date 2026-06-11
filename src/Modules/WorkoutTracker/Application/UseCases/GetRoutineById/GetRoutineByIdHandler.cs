// Path: src/Modules/WorkoutTracker/Application/UseCases/GetRoutineById/GetRoutineByIdHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetRoutineById;
public sealed class GetRoutineByIdHandler(IRoutineRepository repository)
    : IRequestHandler<GetRoutineByIdQuery, ErrorOr<RoutineDto>>
{
    public async Task<ErrorOr<RoutineDto>> Handle(GetRoutineByIdQuery request, CancellationToken ct)
    {
        var r = await repository.GetByIdAsync(request.Id, request.UserId);
        if (r is null) return Error.NotFound("Routine.NotFound", "Routine not found.");
        return new RoutineDto(r.Id, r.Name, r.Description, r.Category, r.DaysOfWeek,
            r.Exercises.Select(e => new ExerciseDto(e.Id, e.Name, e.MuscleGroup, e.Equipment,
                e.Sets, e.Reps, e.WeightKg, e.RestSeconds, e.Notes, e.ExternalId, e.IsImported)).ToList(),
            r.CreatedAt);
    }
}