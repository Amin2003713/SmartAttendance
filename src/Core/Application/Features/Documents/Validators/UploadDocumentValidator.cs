using SmartAttendance.Application.Features.Documents.Requests;

namespace SmartAttendance.Application.Features.Documents.Validators;

// اعتبارسنجی متادیتای سند
public sealed class UploadDocumentValidator : AbstractValidator<UploadDocumentRequest>
{
    public UploadDocumentValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("نام فایل الزامی است.")
            .MaximumLength(255)
            .WithMessage("نام فایل نباید بیش از ۲۵۵ کاراکتر باشد.");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .WithMessage("نوع محتوا الزامی است.")
            .MaximumLength(100)
            .WithMessage("نوع محتوا نباید بیش از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.SizeBytes)
            .GreaterThan(0)
            .WithMessage("حجم فایل باید بزرگ‌تر از صفر باشد.");
    }
}