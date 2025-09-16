using SmartAttendance.Application.Features.Users.Requests.Commands.SendActivationCode;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.SendActivationCode;

public class SendActivationCodeRequestValidator : AbstractValidator<SendActivationCodeRequest>
{
    public SendActivationCodeRequestValidator(IStringLocalizer<SendActivationCodeRequestValidator> localizer)
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["Phone number is required."].Value) // "شماره تلفن الزامی است."
            .Matches(@"^09\d{9}$")
            .WithMessage(localizer["Phone number format is invalid."].Value); // "فرمت شماره تلفن نامعتبر است."
    }
}