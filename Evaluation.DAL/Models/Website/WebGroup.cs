using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Website
{
    public class WebGroup : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string RoutingPath { get; set; } = null!;
        public string BackendName { get; set; } = null!;
    }
}
