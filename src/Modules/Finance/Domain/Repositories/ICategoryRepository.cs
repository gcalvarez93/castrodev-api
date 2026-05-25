// Path: src/Modules/Finance/Domain/Repositories/ICategoryRepository.cs
using Api.Modules.Finance.Domain.Entities;

namespace Api.Modules.Finance.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<Category>> GetAllAsync(string userId);
    Task<string> CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(string id, string userId);
}