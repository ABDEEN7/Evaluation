using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Masters
{
    public class TransactionType : BaseEntities
    {
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

    }
}
