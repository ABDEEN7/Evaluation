using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class SystemTableDTO : EntityBaseDTO
    {
        public string TableNameAr { get; set; } = null!;
        public string TableNameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string DescAr { get; set; } = null!;
        public string DescEn { get; set; } = null!;
        public int? OrderNo { get; set; }
        public ICollection<SystemFieldDTO> SystemFields { get; set; } = new List<SystemFieldDTO>();

    }
}
