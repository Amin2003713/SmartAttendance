using Autofac;
using SmartAttendance.ApiFramework.Controller;
using SmartAttendance.ApiFramework.Injections;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.DynamicTableHelper;
using SmartAttendance.Common.Utilities.EnumHelpers;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Defaults;
using SmartAttendance.Persistence.Db;
using SmartAttendance.Persistence.Jwt;
using SmartAttendance.Persistence.Repositories.Common;
using SmartAttendance.Persistence.Services.Time.ir;


namespace SmartAttendance.ApiFramework.Configuration;

public static class AutofacConfigurationExtensions
{
    public static void RegisterServices(this ContainerBuilder builder)
    {
        RegisterGenericRepositories(builder);
        RegisterAssemblyDependencies(builder);
    }

    private static void RegisterGenericRepositories(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(CommandRepository<>))
            .As(typeof(ICommandRepository<>))
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(QueryRepository<>)).As(typeof(IQueryRepository<>)).InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(TableTranslatorService<>))
            .As(typeof(ITableTranslatorService<>))
            .InstancePerLifetimeScope();
    }

    private static void RegisterAssemblyDependencies(ContainerBuilder builder)
    {
        // Assemblies by concern
        var tenantCommonAssembly = typeof(SmartAttendanceBaseController).Assembly;
        var domainAssembly       = typeof(TenantDefaultValue).Assembly;
        var persistenceAssembly  = typeof(SmartAttendanceDbContext).Assembly;
        var applicationAssembly  = typeof(JwtService).Assembly;
        var frameworkApiAssembly = typeof(AutofacConfigurationExtensions).Assembly;
        var ipaFrameworkAssembly = typeof(WebApiModule).Assembly;
        var ipaModelsAssembly    = typeof(EnumExtensions).Assembly;
        var timeIrAssembly       = typeof(TimeIrService).Assembly;

        var allAssemblies = new[]
        {
            tenantCommonAssembly,
            domainAssembly,
            persistenceAssembly,
            applicationAssembly,
            frameworkApiAssembly,
            ipaFrameworkAssembly,
            ipaModelsAssembly,
            timeIrAssembly
        };

        // Register scoped dependencies
        builder.RegisterAssemblyTypes(allAssemblies)
            .AssignableTo<IScopedDependency>()
            .AsImplementedInterfaces()
            .AsSelf()
            .InstancePerLifetimeScope();

        // Register transient dependencies
        builder.RegisterAssemblyTypes(allAssemblies)
            .AssignableTo<ITransientDependency>()
            .AsImplementedInterfaces()
            .AsSelf()
            .InstancePerDependency();

        // Register singleton dependencies
        builder.RegisterAssemblyTypes(allAssemblies)
            .AssignableTo<ISingletonDependency>()
            .AsImplementedInterfaces()
            .AsSelf()
            .SingleInstance();
    }
}