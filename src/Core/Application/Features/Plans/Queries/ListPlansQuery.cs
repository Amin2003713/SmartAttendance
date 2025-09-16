using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries;

// Query: فهرست طرح‌ها
public sealed record ListPlansQuery : IRequest<IReadOnlyList<PlanDto>>;