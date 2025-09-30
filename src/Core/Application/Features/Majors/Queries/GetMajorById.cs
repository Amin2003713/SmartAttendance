using SmartAttendance.Application.Features.Majors.Responses;

namespace SmartAttendance.Application.Features.Majors.Queries;

public class GetMajorById(
    Guid Id
) : IRequest<GetMajorInfoResponse>;