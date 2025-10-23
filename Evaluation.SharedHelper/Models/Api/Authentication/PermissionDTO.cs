using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.Authentication
{
    public class PermissionDTO : EntityBaseDTO
    {
        public string BackEndName { get; set; } = null!;

        public string? PermissionNameAr { get; set; }

        public string? PermissionNameEn { get; set; }

        public string? Description { get; set; }

    }
}
