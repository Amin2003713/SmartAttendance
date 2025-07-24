using FluentValidation;
using Shifty.Application.Users.Requests.Commands.UpdateUser;

namespace Shifty.Application.Users.Validators.Commands.UpdateUser;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("your email address is not valid.");
    }
}