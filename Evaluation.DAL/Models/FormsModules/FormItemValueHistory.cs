using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.EvalResult;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class FormItemValueHistory : EntityBase, IAuditLogEntity
    {
        public string? SubmissionCodeHistory { get; set; }
        public Guid FormItemValueId { get; set; }
        public FormItemValue? FormItemValue { get; set; }
        public Guid FormItemId { get; set; }
        public FormItem? FormItem { get; set; }
        public Guid UserId { get; set; }
        public MinistryUser? User { get; set; }
        public Guid? DepEvalMatrixId { get; set; }   // Copy Matrid Id
        public DepEvalMatrix? DepEvalMatrix { get; set; }
        public Guid? FormEvalMatrixValueId { get; set; }
        public FormEvalMatrixValue? FormEvalMatrixValue { get; set; }
        public string? DepEvalMatrixValueName { get; set; }   // Copy Text of Matrix Name
        public decimal? ActualValue { get; set; }
        public string? Note { get; set; }
        public Guid? CalcMethodId { get; set; }
        public CalcMethod? CalcMethod { get; set; }
        public string? RenameItem { get; set; }
        public Guid? FormItemConfigId { get; set; }
        public FormItemConfig? FormItemConfig { get; set; }
        public Guid? ServiceRequestId { get; set; }
        public ServiceRequest? ServiceRequest { get; set; }

        public Guid? EvaluationRequestId { get; set; }
        public EvaluationRequest? EvaluationRequest { get; set; }
        public decimal ItemWeight { get; set; } = 0;
        public decimal ItemConfigWeight { get; set; } = 0;
    }
}
