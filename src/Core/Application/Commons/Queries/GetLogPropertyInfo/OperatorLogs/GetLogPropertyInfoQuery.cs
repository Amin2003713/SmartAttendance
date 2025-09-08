using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

namespace SmartAttendance.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;

public record GetLogPropertyInfoQuery(
    Guid Id
) : IRequest<LogPropertyInfoResponse>;