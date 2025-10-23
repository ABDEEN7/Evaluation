using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Generic;

namespace Evaluation.DAL.Entities.BaseModule;

public abstract class EntityBase :  IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool? IsActive { get; set; }
    public Guid? CreateById { get; set; }
    public MinistryUser? CreateBy { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid? UpdateById { get; set; }
    public MinistryUser? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public Guid? DeleteById { get; set; }
    public MinistryUser? DeleteBy { get; set; }
    public DateTime? DeleteDate { get; set; }
    public bool? IsDeleted { get; set; }
}
public interface IViewEntity<T>
{

}