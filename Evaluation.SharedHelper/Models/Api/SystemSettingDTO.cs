using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api
{
    public class SystemSettingDTO : EntityBaseDTO
    {

        public string? SettingGroup { get; set; }
        public string? SettingKey { get; set; }

        public string? SettingValue { get; set; }

        public string? Description { get; set; }
    }
}
