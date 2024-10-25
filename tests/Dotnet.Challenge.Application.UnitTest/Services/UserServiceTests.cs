using Dotnet.Challenge.Application.Services;
using Dotnet.Challenge.Domain.Aggregates.User;
using Dotnet.Challenge.Domain.Exceptions;
using FluentValidation;
using Moq;

namespace Dotnet.Challenge.Application.UnitTest.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<User>> _validatorMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<User>>();
            _userService = new UserService(_userRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidUser_ReturnsUser()
        {
            var user = new User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _userRepositoryMock.Setup(r => r.FindByEmailAsync(user.Email)).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(r => r.AddAsync(user)).ReturnsAsync(user);

            var result = await _userService.CreateAsync(user);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task UpdateAsync_ValidUser_ReturnsUpdatedUser()
        {
            var user = new User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _userRepositoryMock.Setup(r => r.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);

            var result = await _userService.UpdateAsync(user);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingUser_ReturnsUser()
        {
            var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var user = new User(userId, "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));
            _userRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync(user);

            var result = await _userService.GetByIdAsync(userId);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task CreateAsync_UserAlreadyExists_ThrowsUserAlreadyExistException()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _userRepositoryMock.Setup(r => r.FindByEmailAsync(user.Email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UserAlreadyExistException>(() => _userService.CreateAsync(user));
        }

        [Fact]
        public async Task GetByIdAsync_UserNotFound_ReturnsNull()
        {
            var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            _userRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _userService.GetByIdAsync(userId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new User(Guid.NewGuid(), "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24)),
                new User(Guid.NewGuid(), "Rodrigo", "Molinas", "non_existent_email@gmail.com", new DateTime(1991, 1, 1))
            };

            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllAsync();

            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        public async Task DeleteAsync_ExistingUser_DeletesUser()
        {
            var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var user = new User(userId, "Fabricio", "Panaccia", "fake_email@gmail.com", new DateTime(1984, 10, 24));
            _userRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);

            await _userService.DeletAsync(userId);

            _userRepositoryMock.Verify(r => r.DeleteAsync(user), Times.Once);
        }
    }
}
