// Path: src/Modules/WorkoutTracker/Application/UseCases/SearchExternalExercises/SearchExternalExercisesQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.SearchExternalExercises;
public sealed record SearchExternalExercisesQuery(string? MuscleGroup, string? Name) : IRequest<ErrorOr<List<ExternalExerciseDto>>>;