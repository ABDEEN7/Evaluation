using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormBuilder
{
    [Index(nameof(BackendName), IsUnique = true)]
    public class DropDownType : BaseEntities, IAuditLogEntity
    {
        public string BackendName { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string? DataSourceTable { get; set; }
        public Guid? ParentId { get; set; }
        public DropDownType? Parent { get; set; }
    }
}
