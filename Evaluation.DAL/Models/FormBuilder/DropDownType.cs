using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.FormBuilder
{
    [Index(nameof(BackendName), IsUnique = true)]
    public class DropDownType : EntityBase, IAuditLogEntity
    {
        public string BackendName { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string? DataSourceTable { get; set; }
        public Guid? ParentId { get; set; }
        public DropDownType? Parent { get; set; }
        public ICollection<FieldDropDownValue>? FieldDropDownValues { get; set; }
    }
}
