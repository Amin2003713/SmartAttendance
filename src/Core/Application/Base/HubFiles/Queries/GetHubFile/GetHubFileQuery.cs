using SmartAttendance.Application.Base.HubFiles.Request.Queries.GetFile;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.HubFiles.Queries.GetHubFile;

public class GetHubFileQuery : IRequest<FileTransferResponse>
{
    public Guid FileId { get; set; }
    public FileType FileType { get; set; }
    public FileStorageType ReferenceType { get; set; }
    public bool Compress { get; set; }


    public static GetHubFileQuery Create(Guid fileId, FileType fileType, FileStorageType storageType, bool compress)
    {
        return new GetHubFileQuery
        {
            FileId = fileId,
            FileType = fileType,
            ReferenceType = storageType,
            Compress = compress
        };
    }
}