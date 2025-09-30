using SmartAttendance.Application.Features.Subjects.Responses;

public class GetSubjectByIds(
    List<Guid> Ids
) : IRequest<List<GetSubjectInfoResponse>>;