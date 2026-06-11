// Path: src/Modules/WorkoutTracker/Application/UseCases/ImportExternalExercise/ImportExternalExerciseHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.ImportExternalExercise;
public sealed class ImportExternalExerciseHandler(IRoutineRepository repository, ILogger<ImportExternalExerciseHandler> logger)
    : IRequestHandler<ImportExternalExerciseCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(ImportExternalExerciseCommand request, CancellationToken ct)
    {
        var routine = await repository.GetByIdAsync(request.RoutineId, request.UserId);
        if (routine is null) return Error.NotFound("Routine.NotFound", "Routine not found.");

        var exercises = routine.Exercises.ToList();
        exercises.Add(new Exercise {
            Id = Guid.NewGuid().ToString(), Name = request.ExternalExercise.Name,
            MuscleGroup = request.ExternalExercise.MuscleGroup, Equipment = request.ExternalExercise.Equipment,
            Sets = request.Sets, Reps = request.Reps, WeightKg = request.WeightKg,
            RestSeconds = request.RestSeconds, ExternalId = request.ExternalExercise.ExternalId, IsImported = true
        });

        var updated = new Routine
        {
            Id = routine.Id, UserId = routine.UserId, Name = routine.Name,
            Description = routine.Description, Category = routine.Category,
            DaysOfWeek = routine.DaysOfWeek, Exercises = exercises, CreatedAt = routine.CreatedAt
        };
        await repository.UpdateAsync(updated);
        logger.LogInformation("Exercise {Name} imported to routine {RoutineId}", request.ExternalExercise.Name, request.RoutineId);
        return Result.Updated;
    }
}