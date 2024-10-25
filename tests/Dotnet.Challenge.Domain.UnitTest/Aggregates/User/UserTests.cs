using System;

namespace Dotnet.Challenge.Domain.UnitTest.Aggregates.User
{
    public class UserTests
    {
        [Fact]
        public void User_Constructor_Should_Set_Properties_Correctly()
        {
            var id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var firstName = "Fabricio";
            var lastName = "Panaccia";
            var email = "fake_email@gmail.com";
            var dateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z");
            var user = new Domain.Aggregates.User.User(id, firstName, lastName, email, dateOfBirth);

            Assert.Equal(id, user.Id);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
            Assert.Equal(email, user.Email);
            Assert.Equal(dateOfBirth, user.DateOfBirth);
        }

        [Fact]
        public void User_Age_Should_Be_Calculated_Correctly()
        {
            var dateOfBirth = DateTime.Parse("1984-10-24T22:17:32.975Z");
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "fake_email@gmail.com", dateOfBirth);
            var age = user.Age;

            Assert.Equal(DateTime.Today.Year - dateOfBirth.Year - (dateOfBirth.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - dateOfBirth.Year)) ? 1 : 0), age);
        }
    }
}
