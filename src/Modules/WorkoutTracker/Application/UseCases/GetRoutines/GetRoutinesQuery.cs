// Path: src/Modules/WorkoutTracker/Application/UseCases/GetRoutines/GetRoutinesQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetRoutines;
public sealed record GetRoutinesQuery(string UserId) : IRequest<ErrorOr<List<RoutineDto>>>;