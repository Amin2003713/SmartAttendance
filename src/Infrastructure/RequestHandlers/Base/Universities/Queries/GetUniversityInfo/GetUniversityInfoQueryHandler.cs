using SmartAttendance.Application.Base.Universities.Queries.GetUniversityInfo;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;
using SmartAttendance.Application.Interfaces.Settings;
using SmartAttendance.Application.Interfaces.Tenants.Companies;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.Universites.Queries.GetUniversityInfo;

public class GetUniversityInfoQueryHandler(
    IUniversityRepository                           UniversityRepository,
    ISettingQueriesRepository                    settingQueriesRepository,
    IStringLocalizer<GetUniversityInfoQueryHandler> localizer
)
    : IRequestHandler<GetUniversityInfoQuery, GetUniversityInfoResponse>
{
    public async Task<GetUniversityInfoResponse> Handle(GetUniversityInfoQuery request, CancellationToken cancellationToken)
    {
        if (!await UniversityRepository.IdentifierExistsAsync(request.Domain, cancellationToken))
            throw SmartAttendanceException.NotFound(localizer["University not found."].Value); // "شرکت یافت نشد."

        var result  = await UniversityRepository.GetByIdentifierAsync(request.Domain, cancellationToken);
        var setting = await settingQueriesRepository.GetSingleAsync(cancellationToken);


        if (result == null)
            throw SmartAttendanceException.NotFound(localizer["University not found."].Value); // "شرکت یافت نشد."

        return GetUniversityInfoResponse.Create(result, setting);
    }
}