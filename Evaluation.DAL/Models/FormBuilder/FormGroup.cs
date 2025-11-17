using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.ServiceEnities;

namespace Evaluation.DAL.Models.FormBuilder
{
    public class FormGroup : EntityBase, IAuditLogEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string? BackendName { get; set; } 
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; } 
        public int Order { get; set; }
        public Guid FormGroupTypeId { get; set; }
        public FormGroupType? FormGroupType { get; set; }

        public Guid? FormGroupCustomListId { get; set; }
        public FormGroupCustomList? FormGroupCustomList { get; set; }
        public ICollection<Field> Fields { get; set; } =new List<Field>();

    }
}
