// Path: src/Modules/WorkoutTracker/Application/UseCases/UpdateExercise/UpdateExerciseCommand.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.UpdateExercise;
public sealed record UpdateExerciseCommand(string UserId, string RoutineId, string ExerciseId, ExerciseDto Exercise) : IRequest<ErrorOr<Updated>>;