using SmartAttendance.Application.Commons.MediaFiles.Requests;

namespace SmartAttendance.Application.Features.Messages.Request.Commands.CreateMessage;

public class CreateMessageRequest
{
    public string Title { get; set; }

    public string Description { get; set; }


    public UploadMediaFileRequest? ImageFile { get; set; }

    public List<UserMessageResponse>? Recipients { get; set; } = new();
}