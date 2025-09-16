using SmartAttendance.Application.Features.Roles.Requests;

namespace SmartAttendance.Application.Features.Roles.Validators;

public sealed class CreateRoleValidator : AbstractValidator<CreateRoleRequest>
{
	public CreateRoleValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("نام نقش الزامی است.")
			.MinimumLength(2).WithMessage("نام نقش باید حداقل ۲ کاراکتر باشد.")
			.MaximumLength(50).WithMessage("نام نقش نباید بیش از ۵۰ کاراکتر باشد.");
	}
}

