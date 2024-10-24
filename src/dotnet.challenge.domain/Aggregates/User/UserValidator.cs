using FluentValidation;

namespace Dotnet.Challenge.Domain.Aggregates.User
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(user => user.FirstName).MaximumLength(128).WithMessage("FirstName has a maximum length of 128 characters");
            RuleFor(user => user.LastName).MaximumLength(128).WithMessage("LastName has a maximum length of 128 characters");
            RuleFor(user => user.DateOfBirth).NotEmpty().WithMessage("DateOfBirth is required");
            RuleFor(user => user.Age).GreaterThanOrEqualTo(18).WithMessage("User must be older than 18 years").OverridePropertyName("dateOfBirth");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required")
                                       .EmailAddress().WithMessage("Email has an invalid format");
        }
    }
}
