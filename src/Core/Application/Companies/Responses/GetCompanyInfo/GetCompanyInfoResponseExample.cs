using Shifty.Application.Companies.Responses.CompnaySettings;
using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using Shifty.Domain.Setting;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Companies.Responses.GetCompanyInfo;

public class GetCompanyInfoResponseExample : IExamplesProvider<GetCompanyInfoResponse>
{
    public GetCompanyInfoResponse GetExamples()
    {
        // ایجاد نمونهٔ مستقیم از پاسخ با مقادیر فرضی
        return new GetCompanyInfoResponse
        {
            Id = Guid.Empty.ToString(), // Guid.Empty برای مثال
            Domain = "example.com",
            Name = "Example Company",
            LegalName = "Example Co. LLC",
            NationalCode = "1234567890",
            City = "Tehran",
            Province = "Tehran",
            Town = "District 1",
            PostalCode = "1234567890",
            Address = "123 Example St., Tehran",
            PhoneNumber = "02112345678",
            LandLine = "02187654321",
            IsLegal = true,
            Logo = "https://example.com/logo.png",
            ActivityType = "Construction",
            RemainingDays = 365,
            Owner =
                new LogPropertyInfoResponse
                {
                    Id = Guid.Empty,
                    FirstName = "Aghdas",
                    LastName = "nosrat",
                    PhoneNumber = "09134041709",
                    Profile = "https://example.com/Profil.png"
                },
            Settings = new CompanySettingResponse
            {
                Flags = (long)(SettingFlags.CompanyEnabled | SettingFlags.InitialStepper)
            }
        };
    }
}