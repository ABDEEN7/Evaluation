using Evaluation.DAL.Entities.Authentication;

namespace Evaluation.DAL.Entities.Generic;

public interface IEntity
{
    public Guid Id { get; set; }
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