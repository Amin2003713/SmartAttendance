using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries.ListAllByStudentMajor;

public class ListAllByStudentQuery : IRequest<List<GetPlanInfoResponse>>
{
    public Guid StudentId { get; set; }
}