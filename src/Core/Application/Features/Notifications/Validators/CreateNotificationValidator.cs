using SmartAttendance.Application.Features.Notifications.Requests;

namespace SmartAttendance.Application.Features.Notifications.Validators;

// اعتبارسنجی ایجاد اعلان
public sealed class CreateNotificationValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationValidator()
    {
        RuleFor(x => x.RecipientId)
            .NotEmpty()
            .WithMessage("شناسه گیرنده الزامی است.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("عنوان اعلان الزامی است.")
            .MaximumLength(200)
            .WithMessage("عنوان اعلان نباید بیش از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("متن اعلان الزامی است.")
            .MaximumLength(2000)
            .WithMessage("متن اعلان نباید بیش از ۲۰۰۰ کاراکتر باشد.");

        RuleFor(x => x.Channel)
            .NotEmpty()
            .WithMessage("کانال اعلان الزامی است.")
            .Must(c => new[]
            {
                "email",
                "sms",
                "inapp"
            }.Contains(c.Trim().ToLowerInvariant()))
            .WithMessage("کانال اعلان نامعتبر است. (email/sms/inapp)");
    }
}