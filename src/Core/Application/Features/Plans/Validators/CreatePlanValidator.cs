using SmartAttendance.Application.Features.Plans.Requests;

namespace SmartAttendance.Application.Features.Plans.Validators;

// اعتبارسنجی ایجاد طرح
public sealed class CreatePlanValidator : AbstractValidator<CreatePlanRequest>
{
	public CreatePlanValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("عنوان طرح الزامی است.")
			.MinimumLength(3).WithMessage("عنوان طرح باید حداقل ۳ کاراکتر باشد.")
			.MaximumLength(200).WithMessage("عنوان طرح نباید بیش از ۲۰۰ کاراکتر باشد.");

		RuleFor(x => x.Description)
			.MaximumLength(1000).WithMessage("توضیحات نباید بیش از ۱۰۰۰ کاراکتر باشد.");

		RuleFor(x => x.Capacity)
			.GreaterThan(0).WithMessage("ظرفیت باید بیشتر از صفر باشد.");

		RuleFor(x => x)
			.Must(x => x.EndsAtUtc > x.StartsAtUtc)
			.WithMessage("تاریخ پایان باید بعد از تاریخ شروع باشد.");
	}
}

