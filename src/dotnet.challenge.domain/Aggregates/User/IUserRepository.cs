using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet.Challenge.Domain.Aggregates.User
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> FindByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> FindByEmailAsync(string email);
    }
}
