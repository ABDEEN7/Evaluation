using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.StatusDTOs
{
    public class StatusDTO : EntityBaseDTO
    {
        public string ServiceId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public bool IsInitial { get; set; }
        public bool IsOpen { get; set; }
        public string? ColorCode { get; set; }
        public int? OrderNo { get; set; }
    }
}
