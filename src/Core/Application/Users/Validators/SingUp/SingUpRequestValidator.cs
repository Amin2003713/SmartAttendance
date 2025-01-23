using FluentValidation;
using Shifty.Application.Users.Requests.SingUp;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.SingUp
{
    public class SingUpRequestValidator : AbstractValidator<EmployeeSingUpRequest>
    {
        public SingUpRequestValidator(ValidationMessages messages)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(messages.FirstName_Required()).Length(2 , 255).WithMessage(messages.FirstName_Length());

            RuleFor(x => x.LastName).NotEmpty().WithMessage(messages.LastName_Required()).Length(2 , 255).WithMessage(messages.LastName_Length());

            RuleFor(x => x.FatherName).NotEmpty().WithMessage(messages.FatherName_Required()).Length(2 , 255).WithMessage(messages.FatherName_Length());

            RuleFor(x => x.NationalCode).
                NotEmpty().
                WithMessage(messages.NationalCode_Required()).
                Length(10).
                WithMessage(messages.NationalCode_Length()).
                Matches(@"^\d+$").
                WithMessage(messages.NationalCode_Numeric());

            RuleFor(x => x.Gender).NotNull().WithMessage(messages.Gender_Required());

            RuleFor(x => x.IsLeader).NotNull().WithMessage(messages.IsLeader_Required());

            RuleFor(x => x.PhoneNumber).
                NotEmpty().
                WithMessage(messages.PhoneNumber_Required()).
                Matches(@"^09\d{9}$").
                WithMessage(messages.PhoneNumber_InvalidFormat());

            RuleFor(x => x.PersonnelNumber).NotEmpty().WithMessage(messages.PersonnelNumber_Required());

            RuleFor(x => x.DepartmentId).NotNull().WithMessage(messages.DepartmentId_Required());
        }
    }
}