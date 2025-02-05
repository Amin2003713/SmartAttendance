using Autofac;
using Shifty.Common;
using Shifty.Common.General;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Interfaces.Base;
using Shifty.Persistence.Db;
using Shifty.Persistence.Jwt;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.ApiFramework.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public static void RegisterServices(this ContainerBuilder containerBuilder)
        {
            //RegisterType > As > Liftetime
            containerBuilder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            containerBuilder.RegisterGeneric(typeof(EfReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>)).InstancePerLifetimeScope();

            var commonAssembly   = typeof(IScopedDependency).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly     = typeof(AppDbContext).Assembly;
            var servicesAssembly = typeof(JwtService).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly , entitiesAssembly , dataAssembly , servicesAssembly).
                             AssignableTo<IScopedDependency>().
                             AsImplementedInterfaces().AsSelf().
                             InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly , entitiesAssembly , dataAssembly , servicesAssembly).
                             AssignableTo<ITransientDependency>().
                             AsImplementedInterfaces().
                             AsSelf().
                             InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly , entitiesAssembly , dataAssembly , servicesAssembly).
                             AssignableTo<ISingletonDependency>().
                             AsImplementedInterfaces().
                             AsSelf().
                             SingleInstance();
        }
    }
}