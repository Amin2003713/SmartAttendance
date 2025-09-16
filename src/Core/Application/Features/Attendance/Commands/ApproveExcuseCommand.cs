using SmartAttendance.Application.Features.Attendance.Requests;

namespace SmartAttendance.Application.Features.Attendance.Commands;

// Command: تایید معذوریت حضور
public sealed record ApproveExcuseCommand(Guid AttendanceId, ApproveExcuseRequest Request) : IRequest;

public sealed class ApproveExcuseCommandHandler(IAttendanceRepository repo, IUnitOfWork uow) : IRequestHandler<ApproveExcuseCommand>
{
	public async Task Handle(ApproveExcuseCommand request, CancellationToken cancellationToken)
	{
		var att = await repo.GetByIdAsync(new AttendanceId(request.AttendanceId), cancellationToken) ?? throw new KeyNotFoundException("رکورد حضور یافت نشد.");
		att.ApproveExcusal(request.Request.Reason);
		await repo.UpdateAsync(att, cancellationToken);
		await uow.SaveChangesAsync(cancellationToken);
	}

	
}

