using Dotnet.Challenge.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dotnet.Challenge.Application.UnitTest.Commands
{
    public class UpdateUserTests
    {
        [Fact]
        public void ToDomain_ValidUpdateUser_ReturnsUser()
        {
            var updateUser = new UpdateUser
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                FirstName = "Fabricio",
                LastName = "Panaccia",
                Email = "fake_email@gmail.com",
                DateOfBirth = new DateTime(1984, 10, 24)
            };

            var user = updateUser.ToDomain();

            Assert.NotNull(user);
            Assert.Equal(updateUser.Id, user.Id);
            Assert.Equal(updateUser.FirstName, user.FirstName);
            Assert.Equal(updateUser.LastName, user.LastName);
            Assert.Equal(updateUser.Email, user.Email);
            Assert.Equal(updateUser.DateOfBirth, user.DateOfBirth);
        }
    }
}
