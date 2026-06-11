// Path: src/Modules/WorkoutTracker/Application/UseCases/GetSessionById/GetSessionByIdQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetSessionById;
public sealed record GetSessionByIdQuery(string Id, string UserId) : IRequest<ErrorOr<WorkoutSessionDto>>;