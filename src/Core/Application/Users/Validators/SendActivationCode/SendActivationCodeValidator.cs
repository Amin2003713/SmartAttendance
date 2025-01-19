using FluentValidation;
using Shifty.Application.Users.Queries.SendActivationCode;

namespace Shifty.Application.Users.Validators.SendActivationCode
{
    public class SendActivationCodeQueryValidator : AbstractValidator<SendActivationCodeQuery>
    {
        public SendActivationCodeQueryValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("شماره همراه الزامیست").Matches(@"^09\d{9}$").WithMessage("فرمت شماره همراه صحیح نمی‌باشد");
        }
    }
}