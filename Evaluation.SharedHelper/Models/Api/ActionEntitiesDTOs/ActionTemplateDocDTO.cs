using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionTemplateDocDTO : EntityBaseDTO
    {
        public string ActionId { get; set; } = null!;
        public string TemplateDocId { get; set; } = null!;
    }
}
