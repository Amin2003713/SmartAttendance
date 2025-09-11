using Mapster;
using SmartAttendance.Application.Base.Companies.Responses.CompnaySettings;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Domain.Setting;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.Application.Base.Companies.Responses.GetCompanyInfo;

public class GetCompanyInfoResponse
{
    public string? LandLine { get; set; } = null!;

    public string? Address { get; set; } = null!;

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

    public CompanySettingResponse Settings { get; set; }

    public static GetCompanyInfoResponse Create(
        SmartAttendanceTenantInfo companyInfo,
        Setting                   setting)
    {
        var result = companyInfo.Adapt<GetCompanyInfoResponse>();
        result.Settings      = setting.Adapt<CompanySettingResponse>();

        return result;
    }
}