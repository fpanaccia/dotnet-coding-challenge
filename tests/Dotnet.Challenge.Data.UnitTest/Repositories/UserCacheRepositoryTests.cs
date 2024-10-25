using Dotnet.Challenge.Data.Cache;
using Dotnet.Challenge.Data.Repositories;
using Dotnet.Challenge.Domain.Aggregates.User;
using Moq;

namespace Dotnet.Challenge.Data.UnitTest.Repositories
{
    public class UserCacheRepositoryTests
    {
        private readonly Mock<ISimpleObjectCache<Guid, Data.Entities.User>> _mockCache;
        private readonly UserCacheRepository _repository;

        public UserCacheRepositoryTests()
        {
            _mockCache = new Mock<ISimpleObjectCache<Guid, Data.Entities.User>>();
            _repository = new UserCacheRepository(_mockCache.Object);
        }

        [Fact]
        public async Task AddAsync_ValidUser_ShouldAddUserToCache()
        {
            var user = new User(Guid.NewGuid(), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));

            await _repository.AddAsync(user);

            _mockCache.Verify(c => c.AddAsync(user.Id, It.IsAny<Data.Entities.User>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidUser_ShouldDeleteUserFromCache()
        {
            var user = new User(Guid.NewGuid(), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));

            await _repository.DeleteAsync(user);

            _mockCache.Verify(c => c.DeleteAsync(user.Id), Times.Once);
        }

        [Fact]
        public async Task FindByEmailAsync_UserExists_ShouldReturnUser()
        {
            var user = new Data.Entities.User(Guid.NewGuid(), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));
            var users = new List<Data.Entities.User> { user };

            _mockCache.Setup(c => c.GetAllAsync()).ReturnsAsync(users);

            var result = await _repository.FindByEmailAsync("fake_email@gmail.com");

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task FindByEmailAsync_UserDoesNotExist_ShouldReturnNull()
        {
            var users = new List<Data.Entities.User>();
            _mockCache.Setup(c => c.GetAllAsync()).ReturnsAsync(users);
            var result = await _repository.FindByEmailAsync("non_existent_email@gmail.com");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var users = new List<Data.Entities.User>
            {
                new Data.Entities.User(Guid.NewGuid(), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24)),
                new Data.Entities.User(Guid.NewGuid(), "Rodrigo", "Molinas", "non_existent_email@gmail.com", new DateTime(1991, 1, 1))
            };

            _mockCache.Setup(c => c.GetAllAsync()).ReturnsAsync(users);

            var result = await _repository.GetAllAsync();

            Assert.Equal(2, result.Count());
        }
    }
}
