// Path: src/Modules/WorkoutTracker/Application/UseCases/DeleteExercise/DeleteExerciseCommand.cs
using ErrorOr; using MediatR;
namespace Api.Modules.WorkoutTracker.Application.UseCases.DeleteExercise;
public sealed record DeleteExerciseCommand(string UserId, string RoutineId, string ExerciseId) : IRequest<ErrorOr<Deleted>>;