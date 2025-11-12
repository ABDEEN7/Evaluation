using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.LogsDTO
{
    public class ActionTransactionLogDTO
    {

        public Guid Id { get; set; }
        public string? Action { get; set; }
        public string? Actor { get; set; }
        public string? PreviousStatus { get; set; }
        public Guid? PreviousStatusId { get; set; }
        public string? NextStatus { get; set; }
        public Guid? NextStatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FormattedCreatedDate { get; set; }
        public string? Remarks { get; set; }
        public List<AttachementDTO>? ActionTransactionAttachments { get; set; }
    }
}
