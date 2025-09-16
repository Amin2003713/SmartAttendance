using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartAttendance.Application.Abstractions;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Repositories;
using SmartAttendance.Persistence.Mongo.Services;
using SmartAttendance.Persistence.Mongo.UnitOfWork;

namespace SmartAttendance.Persistence.Mongo.DependencyInjection;

// ثبت سرویس‌های MongoDB در DI
public static class MongoPersistenceRegistration
{
	public static IServiceCollection AddMongoPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		var mongoSection = configuration.GetSection("Mongo");
		var options = new MongoDbOptions
		{
			ConnectionString = mongoSection.GetValue<string>("ConnectionString") ?? string.Empty,
			DatabaseName = mongoSection.GetValue<string>("DatabaseName") ?? string.Empty
		};

		services.AddSingleton(new MongoDbContext(options));
		services.AddScoped<IUnitOfWork, MongoUnitOfWork>();

		services.AddScoped<IUserRepository, MongoUserRepository>();
		services.AddScoped<IPlanRepository, MongoPlanRepository>();
		services.AddScoped<IAttendanceRepository, MongoAttendanceRepository>();
		services.AddScoped<IDocumentRepository, MongoDocumentRepository>();
		services.AddScoped<INotificationRepository, MongoNotificationRepository>();
		services.AddScoped<IRoleRepository, MongoRoleRepository>();
		services.AddScoped<IRoleReadService, MongoRoleReadService>();

		return services;
	}
}

