using Evaluation.DAL.Entities.PermissionEntity;

namespace Evaluation.DAL.Entities.BaseModule;

public class ControlValidation
{
    public Guid PermissionId { get; set; }

    public Permission Permission { get; set; } = new();

    public string? Name { get; set; }

    public string? ControlType { get; set; }

    public string? JsonField { get; set; }

    public string? UibackendName { get; set; }

    public bool IsRequired { get; set; }

    public int? MinLength { get; set; }

    public int? MaxLength { get; set; }

    public string? Regex { get; set; }

    public bool? IsDbrequired { get; set; }

    public int? DbMaxLength { get; set; }

    public int? RowOrder { get; set; }

    public int ColumnOrder { get; set; }
    public bool ShowInGrid { get; set; } = false;
    public string? TabulatorConfigJson { get; set; }
    public int? MaxFileCount { get; set; } = 0;
    public int? MaxFileSize { get; set; } = 0;
    public string? FileExtention { get; set; }
    public string? ControlJsonConfig { get; set; }
    public string? ControlAttribute { get; set; }

}
