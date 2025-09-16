namespace SmartAttendance.Persistence.Mongo.Context;

// گزینه‌های اتصال به MongoDB
public sealed class MongoDbOptions
{
	public string ConnectionString { get; init; } = string.Empty;
	public string DatabaseName { get; init; } = string.Empty;
}

