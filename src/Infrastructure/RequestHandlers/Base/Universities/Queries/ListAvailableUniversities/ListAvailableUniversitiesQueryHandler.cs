using Mapster;
using SmartAttendance.Application.Base.Universities.Queries.ListAvailableUniversities;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.RequestHandlers.Base.Universites.Queries.ListAvailableUniversities;

public class ListAvailableUniversitiesQueryHandler(
    IUniversityRepository universityRepository
) : IRequestHandler<ListAvailableUniversitiesQuery , List<GetUniversityInfoResponse>>
{
    public async Task<List<GetUniversityInfoResponse>> Handle(ListAvailableUniversitiesQuery request, CancellationToken cancellationToken)
    {
        var query = await universityRepository.GetAllAsync(cancellationToken);

        return !query.Any() ? [] : query.Adapt<List<GetUniversityInfoResponse>>();
    }
}