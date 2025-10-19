using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Masters
{
    public class TransactionType : EntityBase
    {
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

    }
}
