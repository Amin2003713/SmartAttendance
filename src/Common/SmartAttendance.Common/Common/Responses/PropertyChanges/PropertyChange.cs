namespace SmartAttendance.Common.Common.Responses.PropertyChanges;

public record PropertyChange(
    string PropertyName,
    object? PreviousValue,
    object? CurrentValue,
    DateTime LastModifiedAt,
    string ModifiedBy
);