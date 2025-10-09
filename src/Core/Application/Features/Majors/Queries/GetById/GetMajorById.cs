using SmartAttendance.Application.Features.Majors.Responses;

namespace SmartAttendance.Application.Features.Majors.Queries.GetById;

public record GetMajorById(
    Guid Id
) : IRequest<GetMajorInfoResponse>;