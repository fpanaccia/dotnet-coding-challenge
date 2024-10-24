using Dotnet.Challenge.Domain.Aggregates.User;
using System;

namespace Dotnet.Challenge.Application.Commands
{
    public class CreateUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public User ToDomain()
        {
            return new User(
                Guid.NewGuid(),
                FirstName,
                LastName,
                Email,
                DateOfBirth);
        }
    }
}
