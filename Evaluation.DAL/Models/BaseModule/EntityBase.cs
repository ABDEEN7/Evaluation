using Evaluation.DAL.Models.Generic;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.BaseModule;

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