using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormBuilder
{
    [Index(nameof(BackendName), IsUnique = true)]
    public class CssApplyType : BaseEntities
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;

    }
}
