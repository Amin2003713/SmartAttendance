using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Attendances.Queries.GetByPlan;
using SmartAttendance.Application.Features.Attendances.Responses;
using SmartAttendance.Application.Interfaces.Attendances;

namespace SmartAttendance.Application.Features.Attendances.Queries;

public class GetAttendancesByStudentQueryHandler(
    IAttendanceQueryRepository queryRepository
) : IRequestHandler<GetAttendancesByStudentQuery, List<GetAttendanceInfoResponse>>
{
    public async Task<List<GetAttendanceInfoResponse>> Handle(GetAttendancesByStudentQuery request, CancellationToken cancellationToken)
    {
        var attendances = await queryRepository.TableNoTracking
            .Include(a => a.Enrollment)
            .ThenInclude(e => e.Student)
            .Include(a => a.Excuse)
            .Where(a => a.Enrollment.StudentId == request.StudentId)
            .ToListAsync(cancellationToken);

        return attendances.Adapt<List<GetAttendanceInfoResponse>>();
    }
}