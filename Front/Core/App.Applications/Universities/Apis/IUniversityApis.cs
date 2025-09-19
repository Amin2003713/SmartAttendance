

using Refit;
using SmartAttendance.Application.Base.Universities.Queries.CheckDomain;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;
using SmartAttendance.Application.Features.Users.Queries.GetUserTenants;

namespace SmartAttendance.Api.Client.Refit
{
    public interface IPanelApi
    {
        /// <summary>
        /// Checks the availability or status of a given domain.
        /// </summary>
        [Get("/api/panel/check-domain")]
        Task<ApiResponse<CheckDomainResponse>> CheckDomain([Query] string domain, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the list of tenants associated with a given user.
        /// </summary>
        [Get("/api/panel/user-tenants")]
        Task<ApiResponse<List<GetUserTenantResponse>>> GetUserTenants([Query] string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the list of available universities (tenants).
        /// </summary>
        [Post("/api/panel/get-Tenants")]
        Task<ApiResponse<List<GetUniversityInfoResponse>>> ListAvailableUniversities(CancellationToken cancellationToken = default);
    }
}