using SmartAttendance.Application.Features.Plans.Requests;

namespace SmartAttendance.Application.Features.Plans.Validators;

public sealed class UpdatePlanValidator : AbstractValidator<UpdatePlanRequest>
{
	public UpdatePlanValidator()
	{
		RuleFor(x => x.Title).NotEmpty().WithMessage("عنوان طرح الزامی است.").MinimumLength(3).WithMessage("عنوان طرح باید حداقل ۳ کاراکتر باشد.");
		RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("ظرفیت باید بزرگ‌تر از صفر باشد.");
		RuleFor(x => x).Must(x => x.EndsAtUtc > x.StartsAtUtc).WithMessage("تاریخ پایان باید بعد از تاریخ شروع باشد.");
	}
}

