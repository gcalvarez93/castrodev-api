// Path: src/Modules/WorkoutTracker/Application/UseCases/UpdateRoutine/UpdateRoutineCommand.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.UpdateRoutine;
public sealed record UpdateRoutineCommand(
    string Id, string UserId, string Name, string Description, string Category,
    List<string> DaysOfWeek, List<ExerciseDto> Exercises
) : IRequest<ErrorOr<Updated>>;