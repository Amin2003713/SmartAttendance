using SmartAttendance.Application.Features.Attendances.Responses;

namespace SmartAttendance.Application.Features.Attendances.Queries.GetByPlan;

public class GetAttendancesByStudentQuery : IRequest<List<GetAttendanceInfoResponse>>
{
    public Guid StudentId { get; set; }
}