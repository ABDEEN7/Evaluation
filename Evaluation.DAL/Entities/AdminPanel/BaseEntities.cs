using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Generic;

namespace Evaluation.DAL.Entities.AdminPanel;

public abstract class BaseEntities : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool? IsActive { get; set; }
    public Guid CreateById { get; set; }
    public User? CreateBy { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid? UpdateById { get; set; }
    public User? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public Guid? DeleteById { get; set; }
    public User? DeleteBy { get; set; }
    public DateTime? DeleteDate { get; set; }
    public bool? IsDeleted { get; set; }
}
