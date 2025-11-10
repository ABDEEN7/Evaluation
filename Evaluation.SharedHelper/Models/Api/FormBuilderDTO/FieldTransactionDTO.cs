using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FieldTransactionDTO:EntityBaseDTO
    {
        public string? FieldName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string TransactionType { get; set; } = null!;
        public string ActionName { get; set; } = null!;
        public string TransactionDate { get; set; } = null!;
        public string? Type { get; set; }
        public string? NewAttachmentId { get; set; }
        public string? OldAttachmentId { get; set; }
        public string? User { get; set; }
        public Guid? dropDownTypeId { get; set; }
    }
}
