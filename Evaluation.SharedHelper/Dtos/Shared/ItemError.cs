using Evaluation.SharedHelper.Enums;

namespace Evaluation.SharedHelper.Dtos.Shared;

public class ItemError
{
    public Guid ItemId { get; set; }
    public string? Message { get; set; }
    public ItemPropertyType ItemPropertyType { get; set; }
}
