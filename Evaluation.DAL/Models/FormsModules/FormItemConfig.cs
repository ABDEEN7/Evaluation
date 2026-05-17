using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class FormItemConfig : EntityBase, IAuditLogEntity
    {
        public Guid EvalFormId { get; set; }
        public EvalForm? EvalForm { get; set; }
        public Guid? FormItemId { get; set; }
        public FormItem? FormItem { get; set; }
        public Guid? PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }

        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public Guid CalcMethodId { get; set; }
        public CalcMethod? CalcMethod { get; set; }
        public decimal WeightPercentage { get; set; } = 0;
    }
}
