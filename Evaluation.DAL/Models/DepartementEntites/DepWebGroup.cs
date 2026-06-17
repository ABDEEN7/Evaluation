using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.DepartementEntites
{
    public class DepWebGroup : EntityBase, IAuditLogEntity
    {
        public Guid? WebGroupId { get; set; }
        public WebGroup? WebGroup { get; set; }
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int OrderNo { get; set; } = 0;
    }
}
