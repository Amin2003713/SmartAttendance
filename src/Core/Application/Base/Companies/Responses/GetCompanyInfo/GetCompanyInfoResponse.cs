using Mapster;
using Shifty.Application.Base.Companies.Responses.CompnaySettings;
using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using Shifty.Domain.Setting;
using Shifty.Domain.Tenants;

namespace Shifty.Application.Base.Companies.Responses.GetCompanyInfo;

public class GetCompanyInfoResponse
{
    public string? LandLine { get; set; } = null!;

    public string? Address { get; set; } = null!;

    public LogPropertyInfoResponse Owner { get; set; }

    public string? LegalName { get; set; }

    public string? NationalCode { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? Town { get; set; }

    public string? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }
    public bool IsLegal { get; set; }
    public string? Logo { get; set; }
    public string? ActivityType { get; set; }

    public string Id { get; set; }

    public string Domain { get; set; }
    public string Name { get; set; }

    public long RemainingDays { get; set; }
    public CompanySettingResponse Settings { get; set; }

    public static GetCompanyInfoResponse Create(
        ShiftyTenantInfo companyInfo,
        Setting setting,
        int leftDays,
        LogPropertyInfoResponse owner)
    {
        var result = companyInfo.Adapt<GetCompanyInfoResponse>();
        result.Settings = setting.Adapt<CompanySettingResponse>();
        result.RemainingDays = leftDays;
        result.Owner = owner;
        return result;
    }
}