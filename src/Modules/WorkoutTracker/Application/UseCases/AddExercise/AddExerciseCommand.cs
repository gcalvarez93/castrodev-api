// Path: src/Modules/WorkoutTracker/Application/UseCases/AddExercise/AddExerciseCommand.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.AddExercise;
public sealed record AddExerciseCommand(string UserId, string RoutineId, ExerciseDto Exercise) : IRequest<ErrorOr<Updated>>;