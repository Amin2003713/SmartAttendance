using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries.GetByDate;

public class GetPlanByDateRangeQuery : IRequest<List<GetPlanInfoResponse>>
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}


public class GetDashboardDataResponse
{
    public List<GetPlanInfoResponse> Plans { get; set; } = new();
    public int TotalPlans { get; set; }
    public int TotalEnrollments { get; set; }

    public int WaiteListed { get; set; }
    public int TodayAbsent { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public long TotalStudents { get; set; }
    public long TotalTeachers { get; set; }
    public long TotalHeadMaster { get; set; }
    public long TotalMajor { get; set; }
    public long TotalSubjects { get; set; }
}