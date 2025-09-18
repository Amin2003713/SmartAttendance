using SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;

namespace SmartAttendance.Application.Base.Universities.Commands.InitialUniversity;

public class InitialUniversityCommand : InitialUniversityRequest,
    IRequest<string>;