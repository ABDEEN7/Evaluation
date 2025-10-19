using  Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Logs
{
    public class SMSLog : EntityBase
    {
        public string Mobile { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime? SentDate { get; set; }
        public bool Sent { get; set; }
        public Guid? RefID { get; set; }
    }
}
