using App.Applications.Universities.Queries.CheckDomain;
using App.Applications.Universities.Requests.UpdateCompany;
using App.Applications.Universities.Responses.GetCompanyInfo;
using App.Applications.Users.Queries.GetUserTenants;
using Refit;

namespace App.Applications.Universities.Apis
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
        [Get("/api/panel/get-Tenants")]
        Task<ApiResponse<List<GetUniversityInfoResponse>>> ListAvailableUniversities(CancellationToken cancellationToken = default);
    }


    public interface IUniversityApi
    {
        /// <summary>
        /// Retrieves detailed information about a University by its identifier.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The response containing the University's detailed information.</returns>
        [Get("/api/university/get-info")]
        Task<ApiResponse<GetUniversityInfoResponse>> GetUniversityInfoAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of a University.
        /// </summary>
        /// <param name="request">The update request containing the new University details.</param>
        /// <param name="cancellationToken"></param>
        [Multipart]
        [Put("/api/university/update")]
        Task UpdateUniversityAsync([AliasAs("request")] UpdateUniversityRequest request, CancellationToken cancellationToken = default);
    }
}