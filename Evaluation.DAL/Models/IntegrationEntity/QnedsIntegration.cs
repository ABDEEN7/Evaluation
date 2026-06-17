using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.IntegrationEntity
{
    public class QnedsIntegration : EntityBase, IAuditLogEntity
    {
        public int AcademicYear { get; set; }
        public Guid OrgTreeId { get; set; }
        public OrgTree? OrgTree { get; set; }
        public string JsonValue { get; set; } = null!;
        public string? QnedsConfig { get; set; }
    }
}
