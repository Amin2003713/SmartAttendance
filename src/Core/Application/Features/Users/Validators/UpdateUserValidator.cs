using SmartAttendance.Application.Features.Users.Requests;

namespace SmartAttendance.Application.Features.Users.Validators;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("وارد کردن نام الزامی است.")
            .MinimumLength(2)
            .WithMessage("نام باید حداقل ۲ کاراکتر باشد.")
            .MaximumLength(100)
            .WithMessage("نام نباید بیش از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("وارد کردن نام خانوادگی الزامی است.")
            .MinimumLength(2)
            .WithMessage("نام خانوادگی باید حداقل ۲ کاراکتر باشد.")
            .MaximumLength(100)
            .WithMessage("نام خانوادگی نباید بیش از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("ایمیل الزامی است.")
            .EmailAddress()
            .WithMessage("فرمت ایمیل نامعتبر است.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("شماره تلفن الزامی است.")
            .Matches("^\\+?[0-9]{7,15}$")
            .WithMessage("فرمت شماره تلفن نامعتبر است.");
    }
}