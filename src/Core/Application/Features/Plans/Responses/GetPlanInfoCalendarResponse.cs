using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Features.Plans.Responses;

public class GetPlanInfoCalendarResponse
{
    public string CourseName { get; set; } = default!;

    public string Description { get; set; } = default!;
    public Location Location { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }

    public string Address { get; set; } = default!;

}