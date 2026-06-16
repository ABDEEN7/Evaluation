using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public  class FormEvalMatrixValue : EntityBase , IAuditLogEntity
    {
        public Guid FormEvalMatrixId { get; set; }
        public FormEvalMatrix? FormEvalMatrix { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public decimal MinValue { get; set; } 
        public decimal MaxValue { get; set; }
        public decimal ActualMatrixValue { get; set; }
        public string? DescAr { get; set; }
        public string? DescEn { get; set; }
        public int OrderNo { get; set; } = 0;
        public int NextEvalDays { get; set; } = 0;
        public string? ColorCode { get; set; }
    }
}
