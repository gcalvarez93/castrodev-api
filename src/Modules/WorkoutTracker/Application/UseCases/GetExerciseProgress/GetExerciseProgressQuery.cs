// Path: src/Modules/WorkoutTracker/Application/UseCases/GetExerciseProgress/GetExerciseProgressQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetExerciseProgress;
public sealed record GetExerciseProgressQuery(string UserId, string ExerciseName) : IRequest<ErrorOr<ExerciseProgressDto>>;