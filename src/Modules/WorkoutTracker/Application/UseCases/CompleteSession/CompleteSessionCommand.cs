// Path: src/Modules/WorkoutTracker/Application/UseCases/CompleteSession/CompleteSessionCommand.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.CompleteSession;
public sealed record CompleteSessionCommand(
    string Id, string UserId, List<SessionExerciseDto> Exercises
) : IRequest<ErrorOr<Updated>>;