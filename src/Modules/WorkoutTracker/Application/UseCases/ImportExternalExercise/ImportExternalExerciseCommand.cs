// Path: src/Modules/WorkoutTracker/Application/UseCases/ImportExternalExercise/ImportExternalExerciseCommand.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.ImportExternalExercise;
public sealed record ImportExternalExerciseCommand(string UserId, string RoutineId, ExternalExerciseDto ExternalExercise, int Sets, int Reps, double WeightKg, int RestSeconds) : IRequest<ErrorOr<Updated>>;