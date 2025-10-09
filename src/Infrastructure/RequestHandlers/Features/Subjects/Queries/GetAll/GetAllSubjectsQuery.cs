using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Subjects.Queries.ByIds;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Queries.GetAll;

public class GetSubjectByIdsQueryHandler(
    IdentityService  identityService,
    ISubjectQueryRepository queryRepository
) : IRequestHandler<GetSubjectByIdsQuery , List<GetSubjectInfoResponse>>
{
    public async Task<List<GetSubjectInfoResponse>> Handle(GetSubjectByIdsQuery request, CancellationToken cancellationToken)
    {
        var userRole = identityService.GetRoles();


        return await  queryRepository.TableNoTracking.Where(a => request.Ids.Contains(a.Id))
                   .ProjectToType<GetSubjectInfoResponse>()
                   .ToListAsync(cancellationToken) ??
               [];
    }
}