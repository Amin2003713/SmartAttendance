using SmartAttendance.Application.Commons.MediaFiles.Requests;
using SmartAttendance.Application.Features.Messages.Request.Commands.CreateMessage;

namespace SmartAttendance.Application.Features.Messages.Request.Commands.UpdateMessage;

public class UpdateMessageRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }
    public UploadMediaFileRequest? ImageFile { get; set; }

    public List<UserMessageResponse>? Recipients { get; set; } = new();
}