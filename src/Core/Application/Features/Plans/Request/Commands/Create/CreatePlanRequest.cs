using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Features.Plans.Request.Commands.Create;

public class CreatePlanRequest
{
    public string CourseName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public required List<Guid> TeacherIds { get; set; }
    public required List<Guid> MajorIds { get; set; }
    public Location Location { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
}