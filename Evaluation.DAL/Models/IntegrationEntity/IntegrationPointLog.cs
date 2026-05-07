using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.IntegrationEntity
{
    public class IntegrationPointLog : EntityBase
    {
        public Guid IntegrationPointId { get; set; }
        public IntegrationPoint IntegrationPoint { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
