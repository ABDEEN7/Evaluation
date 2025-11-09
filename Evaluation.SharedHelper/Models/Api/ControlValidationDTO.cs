

using Evaluation.SharedHelper.Models.Api.Authentication;

namespace Evaluation.SharedHelper.Models.Api
{
    public class ControlValidationDTO : EntityBaseDTO
    {


        public Guid PermissionId { get; set; }

        public PermissionDTO Permission { get; set; } = null!;



        public string ControlName { get; set; } = null!;

        public string? ControlType { get; set; }

        public string? JsonFiled { get; set; }

        public string? UibackendName { get; set; }

        public bool IsRequired { get; set; }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        public string? Regex { get; set; }

        public bool? Dbrequired { get; set; }

        public int? DbMaxLength { get; set; }

        public int? RowOrder { get; set; }

        public int ColumnOrder { get; set; }
        public bool ShowInGrid { get; set; } = false;
        public string? TabulatorConfig { get; set; }
        public int? FileCount { get; set; } = 0;
        public int? FileSize { get; set; } = 0;
        public string? FileExtention { get; set; }
        public string? ControlJsonConfig { get; set; }
        public string? ControlAttribute { get; set; }
        public string PermissionName { get; set; } = null!;
    }
}

