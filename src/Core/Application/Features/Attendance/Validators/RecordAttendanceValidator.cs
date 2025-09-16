using SmartAttendance.Application.Features.Attendance.Requests;

namespace SmartAttendance.Application.Features.Attendance.Validators;

// اعتبارسنجی ثبت حضور
public sealed class RecordAttendanceValidator : AbstractValidator<RecordAttendanceRequest>
{
	public RecordAttendanceValidator()
	{
		RuleFor(x => x.AttendanceId)
			.NotEmpty().WithMessage("شناسه حضور الزامی است.");

		RuleFor(x => x.StudentId)
			.NotEmpty().WithMessage("شناسه دانشجو الزامی است.");

		RuleFor(x => x.PlanId)
			.NotEmpty().WithMessage("شناسه طرح الزامی است.");

		When(x => !string.IsNullOrWhiteSpace(x.QrToken), () =>
		{
			RuleFor(x => x.QrExpiresAtUtc)
				.NotNull().WithMessage("تاریخ انقضای QR الزامی است.");
		});

		When(x => x.Latitude.HasValue || x.Longitude.HasValue, () =>
		{
			RuleFor(x => x.Latitude)
				.NotNull().WithMessage("عرض جغرافیایی الزامی است.")
				.InclusiveBetween(-90, 90).WithMessage("عرض جغرافیایی نامعتبر است.");
			RuleFor(x => x.Longitude)
				.NotNull().WithMessage("طول جغرافیایی الزامی است.")
				.InclusiveBetween(-180, 180).WithMessage("طول جغرافیایی نامعتبر است.");
			RuleFor(x => x.AllowedRadiusMeters)
				.NotNull().WithMessage("شعاع مجاز الزامی است.")
				.GreaterThan(0).WithMessage("شعاع مجاز باید بزرگ‌تر از صفر باشد.");
		});
	}
}

