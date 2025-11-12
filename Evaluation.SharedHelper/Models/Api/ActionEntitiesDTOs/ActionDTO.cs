
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.TemplatesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionDTO : EntityBaseDTO
    {
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string BakendName { get; set; } = null!;
        public string ActionTypeBackEndKey { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool IsConfirmationAction { get; set; }
        public bool IsInitialAction { get; set; }
        public int? OrderNo { get; set; }
        public string? ActionTypeId { get; set; }
        public string? ServiceId { get; set; }

        public string Title { get; set; } = null!;
        public string ActionTypeType { get; set; } = null!;
        public List<TempLateDocDTO> Templates { get; set; } = null!;
        public ICollection<ActionFieldDTO> ActionFields { get; } = new List<ActionFieldDTO>();
        public ActionTypeDTO? ActionType { get; set; }
        public bool AllowDraft { get; set; }

    }
}
