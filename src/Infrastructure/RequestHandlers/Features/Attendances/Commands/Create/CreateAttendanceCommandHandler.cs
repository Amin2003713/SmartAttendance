using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Interfaces.Attendances;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.Attendance;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Application.Features.Attendances.Commands;

public class CreateAttendanceCommandHandler(
    IAttendanceCommandRepository commandRepository,
    IPlanEnrollmentQueryRepository enrollmentQueryRepository,
    IPlanQueryRepository planQueryRepository
) : IRequestHandler<CreateAttendanceCommand, Guid>
{
    public async Task<Guid> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
    {
        // Ensure enrollment exists
        var enrollment = await enrollmentQueryRepository.GetByIdAsync(cancellationToken, request.EnrollmentId);
        if (enrollment == null)
            throw SmartAttendanceException.NotFound("Enrollment not found");

        // Prevent duplicate attendance
        if (await commandRepository.TableNoTracking.AnyAsync(a => a.EnrollmentId == request.EnrollmentId, cancellationToken))
            throw SmartAttendanceException.BadRequest("Attendance already exists for this enrollment");

        // Fetch plan start time
        var plan = await planQueryRepository.GetByIdAsync(cancellationToken, enrollment.PlanId);
        if (plan == null)
            throw SmartAttendanceException.NotFound("Plan not found");

        // Calculate status
        AttendanceStatus status;

        if (request.ExcuseId.HasValue)
        {
            status = AttendanceStatus.Excused;
        }
        else
        {
            var now = DateTime.UtcNow;
            status = now <= plan.StartTime.AddMinutes(Math.Round((plan.EndTime - plan.StartTime).TotalMinutes * 0.2))
                ? AttendanceStatus.Present
                : AttendanceStatus.Late;
        }

        var attendance = new Attendance
        {
            Id = Guid.NewGuid(),
            EnrollmentId = request.EnrollmentId,
            Status = status,
            ExcuseId = request.ExcuseId,
            RecordedAt = DateTime.UtcNow
        };

        await commandRepository.AddAsync(attendance, cancellationToken);
        return attendance.Id;
    }
}

