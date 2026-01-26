using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormsModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.ServiceRequestEntities
{
    public class RequestAssignmentScope : EntityBase, IAuditLogEntity
    {
        public Guid RequestAssignmentId { get; set; }
        public RequestAssignment? RequestAssignment { get; set; }
        public Guid ScopeId { get; set; }
        public Scope? Scope { get; set; }
    }
}
