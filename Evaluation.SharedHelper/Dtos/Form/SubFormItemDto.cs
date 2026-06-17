namespace Evaluation.DAL.Dtos.Form;

public class SubFormItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool hasNote { get; set; }
    public List<SubItemList> SubItemLists { get; set; }
}
public class SubItemList
{
    public Guid Id { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public int? OrderNo { get; set; }
}