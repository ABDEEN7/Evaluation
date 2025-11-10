using Scholarship.DAL.Models.Audit;
using Scholarship.DAL.Models.Base;
using Scholarship.SharedHelper.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FormGroupTypeDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
    }
}
