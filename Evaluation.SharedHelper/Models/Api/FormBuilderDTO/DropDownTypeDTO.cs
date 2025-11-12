
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class DropDownTypeDTO : EntityBaseDTO
    {
        public string BackendName { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string? DataSourceTable { get; set; }
        public Guid? ParentId { get; set; }
        public string? Parent { get; set; }
    }
}
