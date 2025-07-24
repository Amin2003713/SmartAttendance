using Shifty.Common.General;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Users.Queries.GetUserTenants;

public class GetUserTenantResponseExample : IExamplesProvider<List<GetUserTenantResponse>>
{
    public List<GetUserTenantResponse> GetExamples()
    {
        return new List<GetUserTenantResponse>
        {
            new()
            {
                Domain = $"tehran.{ApplicationConstant.Const.BaseDomain}", Name = "Tehran co "
            },
            new()
            {
                Domain = $"esf.{ApplicationConstant.Const.BaseDomain}", Name = "Isfahan co "
            }
        };
    }
}