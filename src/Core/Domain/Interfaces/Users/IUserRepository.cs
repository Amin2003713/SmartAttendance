using Shifty.Domain.Interfaces.Base;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Domain.Features.Users;

namespace Shifty.Domain.Interfaces.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateLastLoginDateAsync(User user , CancellationToken cancellationToken);
    }
}