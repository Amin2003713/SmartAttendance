using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries.GetByDate;

public class GetPlanByDateQuery
{
    public DateTime Date { get; set; }
}

public class GetPlanByDateRangeQuery : IRequest<List<GetPlanInfoResponse>>
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}