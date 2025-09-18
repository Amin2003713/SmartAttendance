namespace SmartAttendance.Application.Base.Universities.Queries.IsExist;

public record IsUniversityExistByIdQuery(
    string Id
) : IRequest<bool>;