using Shifty.Domain.Entities;
using Shifty.Domain.Entities.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shifty.Domain.Users
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
