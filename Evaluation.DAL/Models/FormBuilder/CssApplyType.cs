using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormBuilder
{
    [Index(nameof(BackendName), IsUnique = true)]
    public class CssApplyType : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;

    }
}
