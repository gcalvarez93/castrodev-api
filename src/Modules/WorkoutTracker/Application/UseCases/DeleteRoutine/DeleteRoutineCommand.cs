// Path: src/Modules/WorkoutTracker/Application/UseCases/DeleteRoutine/DeleteRoutineCommand.cs
using ErrorOr; using MediatR;
namespace Api.Modules.WorkoutTracker.Application.UseCases.DeleteRoutine;
public sealed record DeleteRoutineCommand(string Id, string UserId) : IRequest<ErrorOr<Deleted>>;