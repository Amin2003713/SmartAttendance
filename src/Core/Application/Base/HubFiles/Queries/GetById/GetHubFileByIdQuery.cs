namespace SmartAttendance.Application.Base.HubFiles.Queries.GetById;

public record GetHubFileByIdQuery(
    Guid Id
) : IRequest<HubFile>;