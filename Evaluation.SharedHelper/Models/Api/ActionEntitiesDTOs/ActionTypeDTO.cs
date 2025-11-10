using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionTypeDTO : EntityBaseDTO
    {
        public string Title { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string BackEndName { get; set; } = null!;
        public int? OrderNo { get; set; }
        public bool? IsRequiredValidation { get; set; }
    }
}
