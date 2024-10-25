using Dotnet.Challenge.Application.Commands;

namespace Dotnet.Challenge.Application.UnitTest.Commands
{
    public class CreateUserTests
    {
        [Fact]
        public void ToDomain_ValidCreateUser_ReturnsUser()
        {
            var createUser = new CreateUser
            {
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = new DateTime(1984, 10, 24)
            };

            var user = createUser.ToDomain();

            Assert.NotNull(user);
            Assert.Equal(createUser.FirstName, user.FirstName);
            Assert.Equal(createUser.LastName, user.LastName);
            Assert.Equal(createUser.Email, user.Email);
            Assert.Equal(createUser.DateOfBirth, user.DateOfBirth);
        }
    }
}
