using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.DepartementEntites
{
    public class DepTargetOrgTree : EntityBase, IAuditLogEntity
    {
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public Guid TargetOrgTreeId { get; set; }
        public OrgTree? TargetOrgTree { get; set; }
        public string? PredicateFuncion { get; set; }
        public string? DepTargetConfig { get; set; }

    }
}
