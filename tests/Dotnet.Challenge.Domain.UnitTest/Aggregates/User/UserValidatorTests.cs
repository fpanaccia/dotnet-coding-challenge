using System;
using Dotnet.Challenge.Domain.Aggregates.User;
using FluentValidation.TestHelper;

namespace Dotnet.Challenge.Domain.UnitTest.Aggregates.User
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator;

        public UserValidatorTests()
        {
            _validator = new UserValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "", "Panaccia", "fake_email@gmail.com", DateTime.Parse("1984-10-24T22:17:32.975Z"));
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Too_Long()
        {
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", new string('A', 129), "fake_email@gmail.com", DateTime.Parse("1984-10-24T22:17:32.975Z"));
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_DateOfBirth_Is_Empty()
        {
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "fake_email@gmail.com", default);
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.DateOfBirth);
        }

        [Fact]
        public void Should_Have_Error_When_Age_Is_Less_Than_18()
        {
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "fake_email@gmail.com", DateTime.Today.AddYears(-10));
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor("dateOfBirth");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "", DateTime.Parse("1984-10-24T22:17:32.975Z"));
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "invalid-email", DateTime.Parse("1984-10-24T22:17:32.975Z"));
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email);
        }

        [Fact]
        public void Should_Not_Have_Error_When_User_Is_Valid()
        {
            var user = new Domain.Aggregates.User.User(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Fabricio", "Panaccia", "fake_email@gmail.com", DateTime.Parse("1984-10-24T22:17:32.975Z"));

            var result = _validator.TestValidate(user);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
