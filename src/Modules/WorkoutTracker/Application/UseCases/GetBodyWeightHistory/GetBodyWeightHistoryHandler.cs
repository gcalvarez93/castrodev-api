// Path: src/Modules/WorkoutTracker/Application/UseCases/GetBodyWeightHistory/GetBodyWeightHistoryHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetBodyWeightHistory;
public sealed class GetBodyWeightHistoryHandler(IBodyWeightRepository repository)
    : IRequestHandler<GetBodyWeightHistoryQuery, ErrorOr<List<BodyWeightDto>>>
{
    public async Task<ErrorOr<List<BodyWeightDto>>> Handle(GetBodyWeightHistoryQuery request, CancellationToken ct)
    {
        var entries = await repository.GetAllAsync(request.UserId);
        return entries.Select(e => new BodyWeightDto(e.Id, e.WeightKg, e.Date, e.Notes, e.CreatedAt)).ToList();
    }
}