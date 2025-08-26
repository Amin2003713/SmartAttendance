using FluentValidation;
using Shifty.Application.Features.Users.Requests.Commands.UpdatePhoneNumber;

namespace Shifty.Application.Features.Users.Validators.Commands.UpdatePhoneNumber;

public class UpdatePhoneNumberRequestValidator : AbstractValidator<UpdatePhoneNumberRequest>
{
    public UpdatePhoneNumberRequestValidator(IStringLocalizer<UpdatePhoneNumberRequestValidator> localizer)
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["PhoneNumber is required."])
            .Matches(@"^09\d{9}$") // برای شماره موبایل ایران
            .WithMessage(localizer["PhoneNumber format is invalid."]);
    }
}