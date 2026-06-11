// Path: src/Modules/WorkoutTracker/Application/UseCases/DeleteExercise/DeleteExerciseHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.DeleteExercise;
public sealed class DeleteExerciseHandler(IRoutineRepository repository)
    : IRequestHandler<DeleteExerciseCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteExerciseCommand request, CancellationToken ct)
    {
        var routine = await repository.GetByIdAsync(request.RoutineId, request.UserId);
        if (routine is null) return Error.NotFound("Routine.NotFound", "Routine not found.");

        var exercises = routine.Exercises.Where(e => e.Id != request.ExerciseId).ToList();

        var updated = new Routine
        {
            Id = routine.Id, UserId = routine.UserId, Name = routine.Name,
            Description = routine.Description, Category = routine.Category,
            DaysOfWeek = routine.DaysOfWeek, Exercises = exercises, CreatedAt = routine.CreatedAt
        };
        await repository.UpdateAsync(updated);
        return Result.Deleted;
    }
}