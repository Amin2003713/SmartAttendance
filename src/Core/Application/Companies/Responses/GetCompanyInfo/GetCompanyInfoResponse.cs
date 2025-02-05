using Mapster;
using Shifty.Domain.Features.Setting;
using Shifty.Domain.Tenants;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Companies.Responces.GetCompanyInfo;

public class GetCompanyInfoResponse
{
    public string Domian { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string LandLine { get; set; }
    public CompanySettingResponse Settings { get; set; }

    public static GetCompanyInfoResponse Create(ShiftyTenantInfo companyInfo , Setting setting)
    {
        var result = companyInfo.Adapt<GetCompanyInfoResponse>();
        result.Settings = setting.Adapt<CompanySettingResponse>();
        return result;
    }
}



    /// <summary>
    /// Provides an example for GetCompanyInfoResponse.
    /// </summary>
    public class GetCompanyInfoResponseExample : IExamplesProvider<GetCompanyInfoResponse>
    {
        /// <summary>
        /// Returns a sample GetCompanyInfoResponse instance for Swagger documentation.
        /// </summary>
        public GetCompanyInfoResponse GetExamples()
        {
            return new GetCompanyInfoResponse
            {
                Domian     = "ipaCo" ,
                Name       = "Ipa soft" ,
                Address    = "Isfahan science sharp fan 1 vaped 105 " ,
                PostalCode = "12345" ,
                LandLine   = "0312332321" ,
                Settings = new CompanySettingResponse
                {
                    Flags = 3 ,
                }
            };
        }
    }

