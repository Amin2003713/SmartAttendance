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
                FirstName          = "John",
                LastName           = "Doe",
                Profile            = "https://example.com/profiles/johndoe.jpg",
                ProfileCompress    = "https://example.com/profiles/johndoe.jpg",
                Address            = "123 Main St, Anytown, USA",
                LastActionOnServer = DateTime.Parse("2025-04-14T12:00:00Z"),
                JobTitle           = "Senior Developer",
                BirthDate          = DateTime.Parse("1990-05-15T00:00:00Z"),
                Id                 = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                IsActive           = true,
                CreatedBy =
                    new LogPropertyInfoResponse
                    {
                        Id      = Guid.Empty,
                        Name    = "Aghdas",
                        Profile = "https://example.com/Profil.png"
                    },
                CreatedAt = DateTime.Parse("2025-04-14T12:00:00Z")
            },
            new()
            {
                FirstName          = "Jane",
                LastName           = "Smith",
                Profile            = "https://example.com/profiles/janesmith.jpg",
                ProfileCompress    = "https://example.com/profiles/janesmith.jpg",
                Address            = "456 Elm St, Somecity, USA",
                LastActionOnServer = DateTime.Parse("2025-04-14T13:00:00Z"),
                JobTitle           = "Product Manager",
                BirthDate          = DateTime.Parse("1985-10-20T00:00:00Z"),
                Id                 = Guid.Parse("4ea85f64-5717-4562-b3fc-2c963f66afa8"),
                IsActive           = true,
                CreatedBy =
                    new LogPropertyInfoResponse
                    {
                        Id   = Guid.Empty,
                        Name = "Aghdas",

                        Profile = "https://example.com/Profil.png"
                    },
                CreatedAt = DateTime.Parse("2025-04-14T13:00:00Z")
            }
        };
    }
}