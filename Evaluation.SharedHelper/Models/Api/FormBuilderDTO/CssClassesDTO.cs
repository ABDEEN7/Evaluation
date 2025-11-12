using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class CssClassesDTO
    {
        public string ClassName { get; set; } = null!;
        public string Styles { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string CssApplyTypeId { get; set; } = null!;
        public CssApplyTypeDTO CssApplyType { get; set; } = null!;
    }
}
