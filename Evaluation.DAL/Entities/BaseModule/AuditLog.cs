namespace Evaluation.DAL.Entities.BaseModule;

public class AuditLog : BaseEntities
{
    public string TableName { get; set; } = null!;
    public string RefId { get; set; } = null!;
    public string ColumnName { get; set; } = null!;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
