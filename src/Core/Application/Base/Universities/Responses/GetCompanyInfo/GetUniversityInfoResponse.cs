using Mapster;
using SmartAttendance.Application.Base.Settings.Queries.GetSetting;
using SmartAttendance.Domain.Setting;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;

public class GetUniversityInfoResponse
{
    public string? LandLine { get; set; }
    public string? Address { get; set; }
    public string? LegalName { get; set; }
    public string? NationalCode { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? Town { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsLegal { get; set; }
    public string? Logo { get; set; }
    public string? UniversityType { get; set; }

    public string Id { get; set; } = null!;
    public string Domain { get; set; } = null!;
    public string Name { get; set; } = null!;

    public GetUniversitySettingResponse Settings { get; set; } = null!;

    public static GetUniversityInfoResponse Create(
        UniversityTenantInfo universityInfo,
        Setting setting)
    {
        var result = universityInfo.Adapt<GetUniversityInfoResponse>();
        result.Settings = setting.Adapt<GetUniversitySettingResponse>();
        return result;
    }
}