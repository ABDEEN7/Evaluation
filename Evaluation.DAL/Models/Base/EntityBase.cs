using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Base
{
    public interface IHasAuditDate
    {
        DateTime CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }
    public interface IViewEntity<T>
    {

    }
    public interface IEntity<T>
    {
        public T Id { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreateById { get; set; }
        public UserProfile? CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? UpdateById { get; set; }
        public UserProfile? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? DeleteById { get; set; }
        public UserProfile? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public abstract class EntityTableBase : IHasAuditDate
    {
        public bool? IsActive { get; set; }
        public Guid CreateById { get; set; }
        public UserProfile? CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? UpdateById { get; set; }
        public UserProfile? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? DeleteById { get; set; }
        public UserProfile? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public abstract class EntityBase : EntityTableBase, IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}
