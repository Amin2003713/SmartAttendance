using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries.GetById;

public class GetByIdQuery : IRequest<GetPlanInfoResponse>
{
    public Guid Id { get; set; } }