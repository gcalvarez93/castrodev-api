// Path: src/Modules/Habits/Domain/Repositories/IHabitRepository.cs
using Api.Modules.Habits.Domain.Entities;

namespace Api.Modules.Habits.Domain.Repositories;

public interface IHabitRepository
{
    Task<Habit?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<Habit>> GetAllAsync(string userId);
    Task<string> CreateAsync(Habit habit);
    Task UpdateAsync(Habit habit);
    Task DeleteAsync(string id, string userId);
}