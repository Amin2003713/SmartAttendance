using SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;
using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Base.Universities.Commands.InitialUniversity;

public class InitialUniversityCommand : InitialUniversityRequest,
    IRequest<string>
{
    public InitialUniversityCommand AddFile(UploadMediaFileRequest requestLogo)
    {
        this.Logo = requestLogo;
        return this;
    }
}