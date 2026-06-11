// Path: src/Modules/WorkoutTracker/Application/UseCases/StartSession/StartSessionCommand.cs
using ErrorOr; using MediatR;
namespace Api.Modules.WorkoutTracker.Application.UseCases.StartSession;
public sealed record StartSessionCommand(string UserId, string RoutineId) : IRequest<ErrorOr<string>>;