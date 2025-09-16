using SmartAttendance.Application.Features.Notifications.Commands;

namespace SmartAttendance.Application.Features.Notifications.Validators;

public sealed class MarkNotificationReadValidator : AbstractValidator<MarkNotificationReadCommand>
{
	public MarkNotificationReadValidator()
	{
		RuleFor(x => x.NotificationId).NotEmpty().WithMessage("شناسه اعلان الزامی است.");
	}
}

