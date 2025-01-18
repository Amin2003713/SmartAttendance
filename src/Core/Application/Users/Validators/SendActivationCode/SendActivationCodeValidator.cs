using FluentValidation;
using Shifty.Application.Users.Requests.SendActivationCode;

namespace Shifty.Application.Users.Validators.SendActivationCode
{
    public class SendActivationCodeValidator : AbstractValidator<SendActivationCodeRequest>
    {

        public SendActivationCodeValidator() =>
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("شماره همراه الزامیست").Matches(@"^09\d{9}$").WithMessage("فرمت شماره همراه صحیح نمی‌باشد");
    }
}