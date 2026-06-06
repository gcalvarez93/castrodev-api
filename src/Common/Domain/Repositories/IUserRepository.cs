// Path: src/Common/Domain/Repositories/IUserRepository.cs
using Api.Common.Domain.Entities;

namespace Api.Common.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
}