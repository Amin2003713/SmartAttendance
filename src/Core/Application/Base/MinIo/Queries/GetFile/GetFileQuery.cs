namespace SmartAttendance.Application.Base.MinIo.Queries.GetFile;

public class GetFileQuery : IRequest<Stream>
{
    public string FilePath { get; set; }
}