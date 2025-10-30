using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models
{
    public class DropdownItem
    {
        public object Id { get; set; }
        public string? Name { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int OrderNo { get; set; }
        public string? ParentId { get; set; }
        public string? FieldType { get; set; }

    }
}
