using SmartAttendance.Application.Features.Attendance.Requests;
using SmartAttendance.Application.Features.Attendance.Responses;
using SmartAttendance.Domain.AttendanceAggregate;
using SmartAttendance.Domain.Services;

namespace SmartAttendance.Application.Features.Attendance.Commands;

// Command: ثبت حضور با یکی از روش‌ها (QR/GPS/Manual/Offline)
public sealed record RecordAttendanceCommand(RecordAttendanceRequest Request) : IRequest<AttendanceDto>;

// Handler: ثبت حضور
public sealed class RecordAttendanceCommandHandler(
	IAttendanceRepository attendanceRepository,
	IAttendanceValidationService validationService,
	IUnitOfWork unitOfWork
) : IRequestHandler<RecordAttendanceCommand, AttendanceDto>
{
	public async Task<AttendanceDto> Handle(RecordAttendanceCommand request, CancellationToken cancellationToken)
	{
		var r = request.Request;
		var id = new AttendanceId(r.AttendanceId);
		var attendance = await attendanceRepository.GetByIdAsync(id, cancellationToken);
		if (attendance is null)
		{
			attendance = new AttendanceAggregate(id, new UserId(r.StudentId), new PlanId(r.PlanId));
			await attendanceRepository.AddAsync(attendance, cancellationToken);
		}

		if (!string.IsNullOrWhiteSpace(r.QrToken) && r.QrExpiresAtUtc.HasValue)
		{
			await attendance.RecordByQrAsync(new QRToken(r.QrToken!, r.QrExpiresAtUtc!.Value), validationService, cancellationToken);
		}
		else if (r.Latitude.HasValue && r.Longitude.HasValue && r.AllowedRadiusMeters.HasValue)
		{
			var student = new GPSCoordinate(r.Latitude!.Value, r.Longitude!.Value);
			var gate = new GPSCoordinate(r.Latitude!.Value, r.Longitude!.Value); // در عمل از تنظیمات طرح خوانده می‌شود
			await attendance.RecordByGpsAsync(student, gate, r.AllowedRadiusMeters!.Value, validationService, cancellationToken);
		}
		else if (r.ApproverId.HasValue)
		{
			await attendance.RecordManualAsync(new UserId(r.ApproverId.Value), validationService, r.ManualPresent ?? true, cancellationToken);
		}
		else
		{
			attendance.RecordOffline();
		}

		await unitOfWork.SaveChangesAsync(cancellationToken);
		return attendance.Adapt<AttendanceDto>();
	}
}

