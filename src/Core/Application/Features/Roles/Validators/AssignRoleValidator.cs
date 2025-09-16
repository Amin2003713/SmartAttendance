using SmartAttendance.Application.Features.Roles.Requests;

namespace SmartAttendance.Application.Features.Roles.Validators;

// اعتبارسنجی انتساب نقش
public sealed class AssignRoleValidator : AbstractValidator<AssignRoleRequest>
{
	public AssignRoleValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty().WithMessage("شناسه کاربر الزامی است.");

		RuleFor(x => x.RoleName)
			.NotEmpty().WithMessage("نام نقش الزامی است.")
			.MinimumLength(2).WithMessage("نام نقش باید حداقل ۲ کاراکتر باشد.")
			.MaximumLength(50).WithMessage("نام نقش نباید بیش از ۵۰ کاراکتر باشد.");
	}
}

