using Dotnet.Challenge.Api.Controllers;
using Dotnet.Challenge.Application.Commands;
using Dotnet.Challenge.Domain.Aggregates.User;
using Dotnet.Challenge.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Dotnet.Challenge.Api.UnitTest.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedUser()
        {
            var user = new CreateUser
            {
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z")
            };

            var domainUser = user.ToDomain();
            _userServiceMock.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(user.ToDomain());

            var result = await _controller.CreateUser(user);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);
            var valueResult = actionResult.Value as User;
            Assert.Equal(domainUser.FirstName, valueResult.FirstName);
            Assert.Equal(domainUser.LastName, valueResult.LastName);
            Assert.Equal(domainUser.Email, valueResult.Email);
            Assert.Equal(domainUser.DateOfBirth, valueResult.DateOfBirth);
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_WhenValidationFails()
        {
            var user = new CreateUser
            {
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("2014-10-24T22:17:32.975Z")
            };

            var validationException = new ValidationException("Validation failed");
            _userServiceMock.Setup(s => s.CreateAsync(It.IsAny<User>())).ThrowsAsync(validationException);

            var result = await _controller.CreateUser(user);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Validation failed", actionResult.Value.GetType().GetProperty("Message").GetValue(actionResult.Value));
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            var user = new CreateUser
            {
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z")
            };

            _userServiceMock.Setup(s => s.CreateAsync(It.IsAny<User>())).ThrowsAsync(new UserAlreadyExistException());

            var result = await _controller.CreateUser(user);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Email already exists", actionResult.Value);
        }

        [Fact]
        public async Task GetUsers_ReturnsListOfUsers()
        {
            var users = new List<User>
        {
            new User(Guid.NewGuid(), "Fabricio", "Panaccia", "fake_email@gmail.com", DateTime.Parse("1984-10-24T22:17:32.975Z"))
        };

            _userServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

            var result = await _controller.GetUsers();

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(users, actionResult.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsUser()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, "Fabricio", "Panaccia", "fake_email@gmail.com", DateTime.Parse("1984-10-24T22:17:32.975Z"));

            _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);

            var result = await _controller.GetUser(userId);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(user, actionResult.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _controller.GetUser(userId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, "Fabricio", "Panaccia", "fake_email@gmail.com", DateTime.Parse("1984-10-24T22:17:32.975Z"));

            _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.DeletAsync(userId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteUser(userId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _controller.DeleteUser(userId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutUser_ReturnsUpdatedUser()
        {
            var userId = Guid.NewGuid();
            var user = new UpdateUser
            {
                Id = userId,
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z")
            };

            var domainUser = user.ToDomain();
            _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(domainUser);

            var result = await _controller.PutUser(userId, user);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(domainUser, actionResult.Value);
        }

        [Fact]
        public async Task PutUser_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var userId = Guid.NewGuid();
            var user = new UpdateUser
            {
                Id = Guid.NewGuid(),
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z")
            };

            var result = await _controller.PutUser(userId, user);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("ID does not correspond with id in object", actionResult.Value);
        }

        [Fact]
        public async Task PutUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var user = new UpdateUser
            {
                Id = userId,
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z")
            };

            _userServiceMock.Setup(s => s.UpdateAsync(user.ToDomain())).ReturnsAsync((User)null);

            var result = await _controller.PutUser(userId, user);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutUser_ReturnsBadRequest_WhenValidationFails()
        {
            var userId = Guid.NewGuid();
            var user = new UpdateUser
            {
                Id = userId,
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("2014-10-24T22:17:32.975Z")
            };

            var validationException = new ValidationException("Validation failed");
            _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<User>())).ThrowsAsync(validationException);

            var result = await _controller.PutUser(userId, user);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Validation failed", actionResult.Value.GetType().GetProperty("Message").GetValue(actionResult.Value));
        }

        [Fact]
        public async Task PutUser_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            var userId = Guid.NewGuid();
            var user = new UpdateUser
            {
                Id = userId,
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z")
            };

            _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<User>())).ThrowsAsync(new UserAlreadyExistException());

            var result = await _controller.PutUser(userId, user);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Email already exists", actionResult.Value);
        }
    }
}
