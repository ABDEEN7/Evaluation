using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormsModules
{
    public  class FormEvalMarixValue : EntityBase , IAuditLogEntity
    {
        public Guid FormEvalMarixid { get; set; }
        public FormEvalMarix? FormEvalMarix { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public decimal MinValue { get; set; } 
        public decimal MaxValue { get; set; }
        public string? DescAr { get; set; }
        public string? DescEn { get; set; }
        public int OrderNo { get; set; } = 0;
    }
}
