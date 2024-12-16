using System;

namespace Shifty.Domain.Entities
{
    public interface IEntity
    {
        bool IsActive { get; set; }

        Guid CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }

        Guid? ModifiedBy { get; set; }
        DateTime ModifiedAt { get; set; }

        Guid? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }

        Guid Id { get; set; }
    }

    public interface ISimpleEntity;

    public abstract class BaseEntity : IEntity
    {
        public bool IsActive { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
    }
}