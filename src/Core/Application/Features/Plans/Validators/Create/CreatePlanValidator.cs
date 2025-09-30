using FluentValidation;
using SmartAttendance.Application.Features.Plans.Request.Commands.Create;

namespace SmartAttendance.Application.Features.Plans.Validators.Create;

public class CreatePlanValidator : AbstractValidator<CreatePlanRequest>
{
    public CreatePlanValidator()
    {
        RuleFor(x => x.CourseName).NotEmpty().WithMessage("نام کلاس الزامی است").MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.MajorIds).NotEmpty().Must(a => a.Count >= 1).WithMessage("انتخاب واحد درسی الزامی است");
        RuleFor(x => x.TeacherIds).NotEmpty().Must(a => a.Count >= 1).WithMessage("انتخاب استاد الزامی است");

        RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("ظرفیت باید بزرگتر از صفر باشد");
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime).WithMessage("زمان شروع باید قبل از زمان پایان باشد");
    }
}