// Path: src/Modules/WorkoutTracker/Application/UseCases/UpdateRoutine/UpdateRoutineHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.UpdateRoutine;
public sealed class UpdateRoutineHandler(IRoutineRepository repository)
    : IRequestHandler<UpdateRoutineCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateRoutineCommand request, CancellationToken ct)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Routine.NotFound", "Routine not found.");
        var updated = new Routine
        {
            Id = request.Id, UserId = request.UserId, Name = request.Name,
            Description = request.Description, Category = request.Category,
            DaysOfWeek = request.DaysOfWeek,
            Exercises = request.Exercises.Select(e => new Exercise {
                Id = string.IsNullOrEmpty(e.Id) ? Guid.NewGuid().ToString() : e.Id,
                Name = e.Name, MuscleGroup = e.MuscleGroup, Equipment = e.Equipment,
                Sets = e.Sets, Reps = e.Reps, WeightKg = e.WeightKg,
                RestSeconds = e.RestSeconds, Notes = e.Notes,
                ExternalId = e.ExternalId, IsImported = e.IsImported
            }).ToList(),
            CreatedAt = existing.CreatedAt
        };
        await repository.UpdateAsync(updated);
        return Result.Updated;
    }
}