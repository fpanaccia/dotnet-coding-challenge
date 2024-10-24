using System;

namespace Dotnet.Challenge.Data.Entities
{
    public class User
    {
        /// <summary>
        /// ID of the User.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the User.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the User.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's D.O.B.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        public User(Guid id, string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
        }
    }
}