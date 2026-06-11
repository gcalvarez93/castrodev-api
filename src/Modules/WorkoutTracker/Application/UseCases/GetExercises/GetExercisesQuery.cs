// Path: src/Modules/WorkoutTracker/Application/UseCases/GetExercises/GetExercisesQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetExercises;
public sealed record GetExercisesQuery(string UserId, string RoutineId) : IRequest<ErrorOr<List<ExerciseDto>>>;