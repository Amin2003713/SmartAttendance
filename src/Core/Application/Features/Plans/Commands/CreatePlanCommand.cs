using SmartAttendance.Application.Features.Plans.Requests;
using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Commands;

// Command: ایجاد طرح
public sealed record CreatePlanCommand(
    CreatePlanRequest Request
) : IRequest<PlanDto>;

// Handler: ایجاد طرح