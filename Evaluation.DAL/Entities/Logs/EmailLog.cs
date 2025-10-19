using  Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Logs
{
    public class EmailLog : EntityBase
    {
        public string Emails { get; set; } = null!;
        public string? CCc { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;

        public DateTime? SentDate { get; set; }
        public bool Sent { get; set; }
        public string? Note { get; set; }
    }
}
