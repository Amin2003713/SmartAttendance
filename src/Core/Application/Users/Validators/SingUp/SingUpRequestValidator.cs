using FluentValidation;
using Shifty.Application.Users.Requests.Commands.SingUp;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.SingUp
{
    public class SingUpRequestValidator : AbstractValidator<EmployeeSingUpRequest>
    {
        public SingUpRequestValidator(ValidationMessages messages)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(messages.FirstName_Required()).Length(2 , 255).WithMessage(messages.FirstName_Length());

            RuleFor(x => x.LastName).NotEmpty().WithMessage(messages.LastName_Required()).Length(2 , 255).WithMessage(messages.LastName_Length());

            RuleFor(x => x.PhoneNumber).
                NotEmpty().
                WithMessage(messages.PhoneNumber_Required()).
                Matches(@"^09\d{9}$").
                WithMessage(messages.PhoneNumber_InvalidFormat());
        }
    }
}