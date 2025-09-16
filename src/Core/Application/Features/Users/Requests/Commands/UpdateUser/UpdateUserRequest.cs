using SmartAttendance.Application.Commons.MediaFiles.Requests;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.UpdateUser;

public class UpdateUserRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public UploadMediaFileRequest? ImageFile { get; set; }

    public string? Address { get; set; } = null!;
    public string? Email { get; set; } = null!;
}