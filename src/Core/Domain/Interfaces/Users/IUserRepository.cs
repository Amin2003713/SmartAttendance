using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Domain.Interfaces.Users
{
    public interface IUserRepository : IRepository<User>
    {
        
            // Task<User?> GetUserByPhoneNumber(string phoneNumber);
            // Task<User?> GetUserById(long userId);

        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}