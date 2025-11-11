using MediatR;
using SmartAttendance.Common.General.Enums.Attendance;

namespace SmartAttendance.Application.Features.Attendances.Commands;

public class CreateAttendanceCommand : IRequest<Guid>
{
    public Guid EnrollmentId { get; set; }
    public Guid? ExcuseId { get; set; } = null;
}