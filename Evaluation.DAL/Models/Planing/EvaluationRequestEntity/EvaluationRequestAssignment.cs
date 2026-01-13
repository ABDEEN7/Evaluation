using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Planing.EvaluationRequestEntity
{
    public class EvaluationRequestAssignment : EntityBase, IAuditLogEntity
    {
        public Guid MinistryUserId { get; set; }
        public MinistryUser? MinistryUser { get; set; }
        public Guid EvaluationRequestId { get; set; }
        public EvaluationRequest? EvaluationRequest { get; set; }
        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public bool IsLeader { get; set; }
        public bool IsNDA { get; set; }
        public Guid? NdaStatusId { get; set; }
        public NdaStatus? NdaStatus { get; set; }
        public DateTime? NdaDate { get; set; }
        public string? Note { get; set; }

    }
}
