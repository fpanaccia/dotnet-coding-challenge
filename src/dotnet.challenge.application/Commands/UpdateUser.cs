using Dotnet.Challenge.Domain.Aggregates.User;
using System;

namespace Dotnet.Challenge.Application.Commands
{
    public class UpdateUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public User ToDomain()
        {
            return new User(
                Id,
                FirstName,
                LastName,
                Email,
                DateOfBirth);
        }
    }
}
