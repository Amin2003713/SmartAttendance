using Shifty.Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        
            // Task<User?> GetUserByPhoneNumber(string phoneNumber);
            // Task<User?> GetUserById(long userId);
       

        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}