
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceDTOs
{

    public class ServiceStatusDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public bool IsInitial { get; set; }
        public bool IsOpen { get; set; }
        public string? ColorCode { get; set; }
        public int? OrderNo { get; set; }
     
        public string ServiceId { get; set; } = null!;
        public ServiceDTO Service { get; set; } = null!;

       

    }
}
