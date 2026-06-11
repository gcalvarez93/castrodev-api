// Path: src/Modules/WorkoutTracker/Domain/Repositories/IBodyWeightRepository.cs
using Api.Modules.WorkoutTracker.Domain.Entities;

namespace Api.Modules.WorkoutTracker.Domain.Repositories;

public interface IBodyWeightRepository
{
    Task<IEnumerable<BodyWeight>> GetAllAsync(string userId);
    Task<string> CreateAsync(BodyWeight bodyWeight);
    Task DeleteAsync(string id, string userId);
}