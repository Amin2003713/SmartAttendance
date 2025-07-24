using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

namespace Shifty.Application.Commons.ItemComment.Create;

public record ItemCommentResponse(
    string Text,
    LogPropertyInfoResponse user,
    DateTime OnDate
);