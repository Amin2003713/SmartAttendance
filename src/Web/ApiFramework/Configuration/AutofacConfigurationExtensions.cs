using Autofac;
using Microsoft.Win32;
using Shifty.ApiFramework.Controller;
using Shifty.ApiFramework.Injections;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.DynamicTableHelper;
using Shifty.Common.Utilities.EnumHelpers;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Defaults;
using Shifty.Domain.Setting;
using Shifty.Persistence.Db;
using Shifty.Persistence.Jwt;
using Shifty.Persistence.Repositories.Common;
using Shifty.Persistence.Services.Time.ir;
using Shifty.RequestHandlers.Base.Discounts.Commands.CreateDiscount;

namespace Shifty.ApiFramework.Configuration;

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
        var tenantCommonAssembly = typeof(IpaBaseController).Assembly;
        var domainAssembly       = typeof(TenantDefaultValue).Assembly;
        var persistenceAssembly  = typeof(ShiftyDbContext).Assembly;
        var applicationAssembly  = typeof(JwtService).Assembly;
        var handlerAssembly      = typeof(CreateDiscountCommandHandler).Assembly;
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
            handlerAssembly,
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