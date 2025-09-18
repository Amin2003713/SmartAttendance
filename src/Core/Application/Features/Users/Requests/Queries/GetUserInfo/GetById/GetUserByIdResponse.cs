namespace SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;

public record GetUserByIdResponse(
    Guid      Id,
    string    FirstName,
    string    LastName,
    string?   FatherName,
    string?   NationalCode,
    string?   ProfileCompress,
    string?   Profile,
    string?   Address,
    string?   Email,
    DateTime? LastActionOnServer,
    DateTime? BirthDate,
    bool      IsActive,
    string?   FullName,
    string?   UniversityDomain,
    string?   UniversityName
);