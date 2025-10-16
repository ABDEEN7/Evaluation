using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormBuilder
{
    public class Attribute : BaseEntities, IAuditLogEntity
    {
        public string Key { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
