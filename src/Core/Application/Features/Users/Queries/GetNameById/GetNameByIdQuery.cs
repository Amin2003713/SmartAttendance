namespace SmartAttendance.Application.Features.Users.Queries.GetNameById;

public record GetNameByIdQuery(
    Guid Id
) : IRequest<string>;