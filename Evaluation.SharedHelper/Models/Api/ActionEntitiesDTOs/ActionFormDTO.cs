using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scholarship.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionFormDTO
    {
        [FromForm]
        public Guid? RequestId { get; set; }
        [FromForm]
        public string? ActionName { get; set; }
        [FromForm]
        public string? ActionRemarks { get; set; }
        [FromForm]
        public IList<FormGroupDTO?>? FormGroupss { get; set; }
        [FromForm(Name = "fieldValues")]
        public List<FieldValueDTO?>? FieldValues { get; set; }

        public IList<AttachementDTO?>? Attachments { get; set; }
        public IList<DropDownValueDTO?>? DropDownValues { get; set; }
        [FromForm]
        public IList<AssignUserDTO?>? Users { get; set; }

        public bool IsRemark { get; set; }
        public bool IsOtherAttachment { get; set; }
        public bool IsRemarkRequired { get; set; }
        public bool IsOtherAttachmentRequired { get; set; }
        public string? RemarkLabel { get; set; }
        public string? AttachmentLabel { get; set; }
        public bool IsIntialStatus { get; set; }
    }
}
