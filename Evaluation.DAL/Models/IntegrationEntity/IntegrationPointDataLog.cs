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
    public class IntegrationPointDataLog : EntityBase
    {
        public Guid IntegrationPointLogId { get; set; }
        public IntegrationPointLog? IntegrationPointLog { get; set; }
        public Guid? OrgTreeId { get; set; }
        public OrgTree? OrgTree { get; set; }
        public string DataResponse { get; set; } = null!;
        public string? Note { get; set; }
    }
}
