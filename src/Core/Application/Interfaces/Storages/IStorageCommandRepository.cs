using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;

namespace Shifty.Application.Interfaces.Storages;

public interface IStorageCommandRepository : ICommandRepository<Domain.Storages.Storage>,
    IScopedDependency
{
}