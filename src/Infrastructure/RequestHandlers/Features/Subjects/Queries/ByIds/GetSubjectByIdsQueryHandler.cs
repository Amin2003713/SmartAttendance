using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Subjects.Queries;
using SmartAttendance.Application.Features.Subjects.Queries.ByIds;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Application.Interfaces.Majors;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Queries.ByIds;

public class GetSubjectByIdsQueryHandler(
    ISubjectQueryRepository queryRepository
) : IRequestHandler<GetSubjectByIdsQuery , List<GetSubjectInfoResponse>>
{
    public async Task<List<GetSubjectInfoResponse>> Handle(GetSubjectByIdsQuery request, CancellationToken cancellationToken)
    {
        return await  queryRepository.TableNoTracking.Where(a => request.Ids.Contains(a.Id))
                   .ProjectToType<GetSubjectInfoResponse>()
                   .ToListAsync(cancellationToken) ??
               [];
    }
}