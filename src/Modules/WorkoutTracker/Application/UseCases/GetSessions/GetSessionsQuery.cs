// Path: src/Modules/WorkoutTracker/Application/UseCases/GetSessions/GetSessionsQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetSessions;
public sealed record GetSessionsQuery(string UserId, string? RoutineId) : IRequest<ErrorOr<List<WorkoutSessionDto>>>;