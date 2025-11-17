using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.FormBuilder
{
    [Index(nameof(BackendName), IsUnique = true)]
    public class CssApplyType : EntityBase
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;

    }
}
