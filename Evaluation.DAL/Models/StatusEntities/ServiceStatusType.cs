using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.StatusEntities
{
    [Index(nameof(BackendName),IsUnique =true)]
    public class ServiceStatusType : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEN { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public bool IsOpen { get; set; }
        public string ColorCode { get; set; }= null!;
        public int OrderNo { get; set; }
    }
}
