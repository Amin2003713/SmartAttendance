using FluentValidation;
using Shifty.Application.Features.Users.Requests.Commands.UpdateUser;

namespace Shifty.Application.Features.Users.Validators.Commands.UpdateUser;

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