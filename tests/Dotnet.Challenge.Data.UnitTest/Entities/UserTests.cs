using Dotnet.Challenge.Data.Entities;

namespace Dotnet.Challenge.Data.UnitTest.Entities
{
    public class UserTests
    {
        [Fact]
        public void User_Creation_ValidParameters_ShouldCreateUser()
        {
            var id = Guid.NewGuid();
            var firstName = "Fabricio";
            var lastName = "Panaccia";
            var email = "fake_email@gmail.com";
            var dateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z");

            var user = new User(id, firstName, lastName, email, dateOfBirth);

            Assert.Equal(id, user.Id);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
            Assert.Equal(email, user.Email);
            Assert.Equal(dateOfBirth, user.DateOfBirth);
        }
    }
}
