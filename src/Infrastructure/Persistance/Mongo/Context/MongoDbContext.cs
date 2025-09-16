using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SmartAttendance.Persistence.Mongo.Documents;

namespace SmartAttendance.Persistence.Mongo.Context;

// کانتکست MongoDB: نگهداری دیتابیس و کالکشن‌ها
public sealed class MongoDbContext
{
	public IMongoDatabase Database { get; }
	public IMongoCollection<UserDocument> Users { get; }
	public IMongoCollection<PlanDocument> Plans { get; }
	public IMongoCollection<AttendanceDocument> Attendances { get; }
	public IMongoCollection<DocumentDocument> Documents { get; }
	public IMongoCollection<NotificationDocument> Notifications { get; }
	public IMongoCollection<RoleDocument> Roles { get; }

	public MongoDbContext(MongoDbOptions options)
	{
		if (string.IsNullOrWhiteSpace(options.ConnectionString))
			throw new ArgumentException("MongoDb connection string is required.", nameof(options.ConnectionString));
		if (string.IsNullOrWhiteSpace(options.DatabaseName))
			throw new ArgumentException("MongoDb database name is required.", nameof(options.DatabaseName));

		BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());
		BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

		var client = new MongoClient(options.ConnectionString);
		Database = client.GetDatabase(options.DatabaseName);

		Users = Database.GetCollection<UserDocument>("users");
		Plans = Database.GetCollection<PlanDocument>("plans");
		Attendances = Database.GetCollection<AttendanceDocument>("attendances");
		Documents = Database.GetCollection<DocumentDocument>("documents");
		Notifications = Database.GetCollection<NotificationDocument>("notifications");
		Roles = Database.GetCollection<RoleDocument>("roles");

		EnsureIndexes();
	}

	private void EnsureIndexes()
	{
		Users.Indexes.CreateMany(new[]
		{
			new CreateIndexModel<UserDocument>(Builders<UserDocument>.IndexKeys.Ascending(x => x.Email), new CreateIndexOptions{ Unique = true, Name = "ux_users_email" }),
			new CreateIndexModel<UserDocument>(Builders<UserDocument>.IndexKeys.Ascending(x => x.NationalCode), new CreateIndexOptions{ Unique = true, Name = "ux_users_ncode" })
		});

		Plans.Indexes.CreateOne(new CreateIndexModel<PlanDocument>(
			Builders<PlanDocument>.IndexKeys.Ascending(x => x.StartsAtUtc).Ascending(x => x.EndsAtUtc),
			new CreateIndexOptions { Name = "ix_plans_time" }));

		Attendances.Indexes.CreateMany(new[]
		{
			new CreateIndexModel<AttendanceDocument>(Builders<AttendanceDocument>.IndexKeys.Ascending(x => x.StudentId).Ascending(x => x.PlanId), new CreateIndexOptions { Name = "ix_att_student_plan" }),
			new CreateIndexModel<AttendanceDocument>(Builders<AttendanceDocument>.IndexKeys.Ascending(x => x.RecordedAtUtc), new CreateIndexOptions { Name = "ix_att_recorded" })
		});

		Roles.Indexes.CreateOne(new CreateIndexModel<RoleDocument>(
			Builders<RoleDocument>.IndexKeys.Ascending(x => x.Name),
			new CreateIndexOptions { Unique = true, Name = "ux_roles_name" }));
	}

	private sealed class GuidSerializationProvider : IBsonSerializationProvider
	{
		public IBsonSerializer? GetSerializer(Type type)
		{
			if (type == typeof(Guid)) return new GuidSerializer(GuidRepresentation.Standard);
			return null;
		}
	}
}

