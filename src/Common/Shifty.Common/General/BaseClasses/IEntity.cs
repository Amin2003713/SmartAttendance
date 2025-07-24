namespace Shifty.Domain.Commons.BaseClasses;

public interface IEntity
{
    bool IsActive { get; set; }

    Guid? CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }

    Guid? ModifiedBy { get; set; }
    DateTime? ModifiedAt { get; set; }

    Guid? DeletedBy { get; set; }
    DateTime? DeletedAt { get; set; }

    Guid Id { get; set; }
}