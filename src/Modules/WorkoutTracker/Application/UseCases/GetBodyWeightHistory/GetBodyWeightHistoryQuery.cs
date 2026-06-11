// Path: src/Modules/WorkoutTracker/Application/UseCases/GetBodyWeightHistory/GetBodyWeightHistoryQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetBodyWeightHistory;
public sealed record GetBodyWeightHistoryQuery(string UserId) : IRequest<ErrorOr<List<BodyWeightDto>>>;