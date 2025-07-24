using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

namespace Shifty.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;

public record GetLogPropertyInfoQuery(
    Guid Id
) : IRequest<LogPropertyInfoResponse>;