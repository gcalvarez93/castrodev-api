// Path: src/Modules/WorkoutTracker/Application/UseCases/CreateRoutine/CreateRoutineCommand.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.CreateRoutine;
public sealed record CreateRoutineCommand(
    string UserId, string Name, string Description, string Category,
    List<string> DaysOfWeek, List<ExerciseDto> Exercises
) : IRequest<ErrorOr<string>>;