using System.Threading;
using System.Threading.Tasks;
using Shifty.Domain.Features.Users;
using Shifty.Domain.Interfaces.Base;

namespace Shifty.Domain.Interfaces.Features.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateLastLoginDateAsync(User user , CancellationToken cancellationToken);
    }
}