using SmartAttendance.Application.Base.Companies.Responses.CompnaySettings;
using SmartAttendance.Domain.Setting;

namespace SmartAttendance.Application.Base.Companies.Responses.GetCompanyInfo;

public class GetCompanyInfoResponseExample : IExamplesProvider<GetCompanyInfoResponse>
{
    public GetCompanyInfoResponse GetExamples()
    {
        // ایجاد نمونهٔ مستقیم از پاسخ با مقادیر فرضی
        return new GetCompanyInfoResponse
        {
            Id           = Guid.Empty.ToString(), // Guid.Empty برای مثال
            Domain       = "example.com",
            Name         = "Example Company",
            LegalName    = "Example Co. LLC",
            NationalCode = "1234567890",
            City         = "Tehran",
            Province     = "Tehran",
            Town         = "District 1",
            PostalCode   = "1234567890",
            Address      = "123 Example St., Tehran",
            PhoneNumber  = "02112345678",
            LandLine     = "02187654321",
            IsLegal      = true,
            Logo         = "https://example.com/logo.png",
            ActivityType = "Construction",

            Settings = new CompanySettingResponse
            {
                Flags = (long)(SettingFlags.CompanyEnabled | SettingFlags.InitialStepper)
            }
        };
    }
}