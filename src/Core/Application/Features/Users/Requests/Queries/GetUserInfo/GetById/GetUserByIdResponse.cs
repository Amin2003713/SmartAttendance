namespace SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;

public record GetUserByIdResponse(
    Guid      Id,
    string    FirstName,
    string    LastName,
    string?   ProfileCompress,
    string?   Profile,
    string?   Address,
    DateTime? LastLoginDate,
    DateTime? BirthDate
);