using Dotnet.Challenge.Data.Cache;
using Dotnet.Challenge.Domain.Aggregates.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet.Challenge.Data.Repositories
{
    public class UserCacheRepository : IUserRepository
    {
        private readonly ISimpleObjectCache<Guid, Data.Entities.User> _userCache;

        public UserCacheRepository(ISimpleObjectCache<Guid, Data.Entities.User> userCache)
        {
            _userCache = userCache;
        }

        public async Task<User> AddAsync(User user)
        {
            var newUser = MapToEntity(user);
            await _userCache.AddAsync(newUser.Id, newUser);
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            await _userCache.DeleteAsync(user.Id);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var allUsers = await _userCache.GetAllAsync();
            var userOnCache = allUsers.FirstOrDefault(x => x.Email == email);
            return userOnCache == null ? null : MapToDomain(userOnCache);
        }

        public async Task<User?> FindByIdAsync(Guid id)
        {
            var userOnCache = await _userCache.GetAsync(id);
            return userOnCache == null ? null : MapToDomain(userOnCache);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var allUsers = await _userCache.GetAllAsync();
            return allUsers.Select(x => MapToDomain(x));
        }

        public async Task<User> UpdateAsync(User user)
        {
            var updatedUser = MapToEntity(user);
            await _userCache.UpdateAsync(updatedUser.Id, updatedUser);
            return user;
        }

        public static User MapToDomain(Entities.User user)
        {
            return new User(
                id: user.Id,
                firstName: user.FirstName,
                lastName: user.LastName,
                email: user.Email,
                dateOfBirth: user.DateOfBirth
            );
        }

        public static Entities.User MapToEntity(User user)
        {
            return new Entities.User(
                id: user.Id,
                firstName: user.FirstName,
                lastName: user.LastName,
                email: user.Email,
                dateOfBirth: user.DateOfBirth
            );
        }
    }
}
