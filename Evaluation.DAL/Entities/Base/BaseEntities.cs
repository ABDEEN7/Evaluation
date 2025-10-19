using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Generic;

<<<<<<<< HEAD:Evaluation.DAL/Entities/Base/BaseEntities.cs
<<<<<<<< HEAD:Evaluation.DAL/Entities/Base/BaseEntities.cs
namespace Evaluation.DAL.Entities.Base;
========
namespace Evaluation.DAL.Entities.BaseModule;
>>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5:Evaluation.DAL/Entities/BaseModule/EntityBase.cs
========
namespace Evaluation.DAL.Entities.BaseModule;
>>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5:Evaluation.DAL/Entities/BaseModule/EntityBase.cs

public abstract class EntityBase : IEntity
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
