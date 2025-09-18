using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;

namespace SmartAttendance.Application.Features.Users.Requests.Queries.GetAllUsers;

public class GetAllUsersResponseExample : IExamplesProvider<List<GetUserResponse>>
{
    public List<GetUserResponse> GetExamples()
    {
        return new List<GetUserResponse>
        {
            new()
            {
                Id                 = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                FirstName          = "John",
                LastName           = "Doe",
                FatherName         = "Michael",
                NationalCode       = "1234567890",
                ProfilePicture            = "https://example.com/profiles/johndoe.jpg",
                Address            = "123 Main St, Anytown, USA",
                Email              = "johndoe@example.com",
                LastActionOnServer = DateTime.Parse("2025-04-14T12:00:00Z"),
                BirthDate          = DateTime.Parse("1990-05-15T00:00:00Z"),
                IsActive           = true,
                FullName           = "John Doe",
                UniversityDomain   = "tehranuni.edu",
                UniversityName     = "Tehran University",
                CreatedBy          = new LogPropertyInfoResponse
                {
                    Id      = Guid.Empty,
                    Name    = "Admin User",
                    Profile = "https://example.com/profile_admin.png"
                },
                CreatedAt = DateTime.Parse("2025-04-14T12:00:00Z")
            },
            new()
            {
                Id                 = Guid.Parse("4ea85f64-5717-4562-b3fc-2c963f66afa8"),
                FirstName          = "Jane",
                LastName           = "Smith",
                FatherName         = "Robert",
                NationalCode       = "0987654321",
                ProfilePicture            = "https://example.com/profiles/janesmith.jpg",
                Address            = "456 Elm St, Somecity, USA",
                Email              = "janesmith@example.com",
                LastActionOnServer = DateTime.Parse("2025-04-14T13:00:00Z"),
                BirthDate          = DateTime.Parse("1985-10-20T00:00:00Z"),
                IsActive           = true,
                FullName           = "Jane Smith",
                UniversityDomain   = "azaduni.edu",
                UniversityName     = "Azad University",
                CreatedBy          = new LogPropertyInfoResponse
                {
                    Id      = Guid.Empty,
                    Name    = "Admin User",
                    Profile = "https://example.com/profile_admin.png"
                },
                CreatedAt = DateTime.Parse("2025-04-14T13:00:00Z")
            }
        };
    }
}