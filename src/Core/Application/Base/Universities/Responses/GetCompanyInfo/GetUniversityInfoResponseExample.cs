using SmartAttendance.Application.Base.Settings.Queries.GetSetting;
using SmartAttendance.Domain.Setting;

namespace SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;

public class GetUniversityInfoResponseExample : IExamplesProvider<GetUniversityInfoResponse>
{
    public GetUniversityInfoResponse GetExamples()
    {
        // نمونه واقعی از دانشگاه آزاد اصفهان (خوارسگان)
        return new GetUniversityInfoResponse
        {
            Id           = Guid.NewGuid().ToString(),
            Domain       = "iau-esfahan",
            Name         = "Islamic Azad University, Esfahan (Khorasgan) Branch",
            LegalName    = "Islamic Azad University",
            City         = "Esfahan",
            Province     = "Esfahan",
            Town         = "Khorasgan",
            PostalCode   = "81595-158",
            Address      = "Arghavanieh Blvd, Khorasgan, Esfahan, Iran",
            PhoneNumber  = "03135354000",
            LandLine     = "03135354001",
            IsLegal      = true,
            Logo         = "https://iau-esfahan.ac.ir/logo.png",
            UniversityType = "Private", // یا "Azad"

            Settings = new GetUniversitySettingResponse
            {
                Flags = SettingFlags.UniversityEnabled | SettingFlags.InitialStepper
            }
        };
    }
}