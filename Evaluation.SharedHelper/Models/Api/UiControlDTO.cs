using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api
{
    public class UiControlDTO : EntityBaseDTO
    {

        public string? PageName { get; set; }

        public string? UserUiname { get; set; }

        public string? BackEndName { get; set; }

        public string? ControlName { get; set; }

        public string? EnValue { get; set; }

        public string? ArValue { get; set; }

        public string? Url { get; set; }
        public string? txtValue { get; set; }
    }
}
