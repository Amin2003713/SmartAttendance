using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Plans.Queries.GetById;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class GetByIdQueryHandler(
    IPlanQueryRepository queryRepository
) : IRequestHandler<GetByIdQuery, GetPlanInfoResponse>
{
    public async Task<GetPlanInfoResponse> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await queryRepository.TableNoTracking
            .Include(a => a.Subjects)
            .Include(a => a.Teacher)
            .Include(a => a.Major)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (plan == null)
            throw SmartAttendanceException.NotFound("Plan not found");

        return plan.Adapt<GetPlanInfoResponse>();
    }
}