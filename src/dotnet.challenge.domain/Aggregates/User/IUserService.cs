using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Dotnet.Challenge.Domain.Aggregates.User
{
    public interface IUserService
    {
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task DeletAsync(Guid userId);
        Task<User?> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
