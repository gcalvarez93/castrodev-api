// Path: src/Modules/WorkoutTracker/Application/UseCases/GetRoutineById/GetRoutineByIdQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetRoutineById;
public sealed record GetRoutineByIdQuery(string Id, string UserId) : IRequest<ErrorOr<RoutineDto>>;