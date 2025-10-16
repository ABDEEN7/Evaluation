using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormBuilder
{
    public class FieldDropDownValue : BaseEntities, IAuditLogEntity
    {

        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string? DropDownBackendName { get; set; }
        public Guid DropDownTypeId { get; set; }
        public DropDownType? dropDownType { get; set; }
        public Guid? ParentDropDownId { get; set; }
        public FieldDropDownValue? ParentDropDown { get; set; }
        public int? OrderNo { get; set; }
    }
}
