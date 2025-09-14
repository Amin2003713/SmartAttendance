using SmartAttendance.Common.General;

namespace SmartAttendance.Application.Features.Users.Queries.GetUserTenants;

public class GetUserTenantResponseExample : IExamplesProvider<List<GetUserTenantResponse>>
{
    public List<GetUserTenantResponse> GetExamples()
    {
        return new List<GetUserTenantResponse>
        {
            new GetUserTenantResponse
            {
                Domain = $"tehran.{ApplicationConstant.Const.BaseDomain}",
                Name   = "Tehran co "
            },
            new GetUserTenantResponse
            {
                Domain = $"esf.{ApplicationConstant.Const.BaseDomain}",
                Name   = "Isfahan co "
            }
        };
    }
}