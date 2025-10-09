using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Subjects.Queries.ByIds;
using SmartAttendance.Application.Features.Subjects.Queries.GetAll;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Queries.GetAll;

public class GetAllSubjectsQueryHandler(
    IdentityService  identityService,
    ISubjectQueryRepository queryRepository
) : IRequestHandler<GetAllSubjectsQuery , List<GetSubjectInfoResponse>>
{
    public async Task<List<GetSubjectInfoResponse>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
    {
        var userRole = identityService.GetRoles();


        return await  queryRepository.TableNoTracking
                   .ProjectToType<GetSubjectInfoResponse>()
                   .ToListAsync(cancellationToken) ??
               [];
    }
}