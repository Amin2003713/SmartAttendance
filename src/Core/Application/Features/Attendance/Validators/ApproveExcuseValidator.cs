using SmartAttendance.Application.Features.Attendance.Requests;

namespace SmartAttendance.Application.Features.Attendance.Validators;

public sealed class ApproveExcuseValidator : AbstractValidator<ApproveExcuseRequest>
{
    public ApproveExcuseValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().WithMessage("علت معذوریت الزامی است.").MinimumLength(3).WithMessage("علت معذوریت بسیار کوتاه است.");
    }
}