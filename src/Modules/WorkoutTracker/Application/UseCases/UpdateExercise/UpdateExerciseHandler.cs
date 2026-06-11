/// Path: src/Modules/WorkoutTracker/Application/UseCases/UpdateExercise/UpdateExerciseHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.UpdateExercise;
public sealed class UpdateExerciseHandler(IRoutineRepository repository)
    : IRequestHandler<UpdateExerciseCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateExerciseCommand request, CancellationToken ct)
    {
        var routine = await repository.GetByIdAsync(request.RoutineId, request.UserId);
        if (routine is null) return Error.NotFound("Routine.NotFound", "Routine not found.");

        var exercises = routine.Exercises.ToList();
        var idx = exercises.FindIndex(e => e.Id == request.ExerciseId);
        if (idx < 0) return Error.NotFound("Exercise.NotFound", "Exercise not found.");

        exercises[idx] = new Exercise {
            Id = request.ExerciseId, Name = request.Exercise.Name,
            MuscleGroup = request.Exercise.MuscleGroup, Equipment = request.Exercise.Equipment,
            Sets = request.Exercise.Sets, Reps = request.Exercise.Reps,
            WeightKg = request.Exercise.WeightKg, RestSeconds = request.Exercise.RestSeconds,
            Notes = request.Exercise.Notes, ExternalId = exercises[idx].ExternalId,
            IsImported = exercises[idx].IsImported
        };

        var updated = new Routine
        {
            Id = routine.Id, UserId = routine.UserId, Name = routine.Name,
            Description = routine.Description, Category = routine.Category,
            DaysOfWeek = routine.DaysOfWeek, Exercises = exercises, CreatedAt = routine.CreatedAt
        };
        await repository.UpdateAsync(updated);
        return Result.Updated;
    }
}