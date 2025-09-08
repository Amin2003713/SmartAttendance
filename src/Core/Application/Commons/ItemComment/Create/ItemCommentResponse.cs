using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

namespace SmartAttendance.Application.Commons.ItemComment.Create;

public record ItemCommentResponse(
    string Text,
    LogPropertyInfoResponse user,
    DateTime OnDate
);