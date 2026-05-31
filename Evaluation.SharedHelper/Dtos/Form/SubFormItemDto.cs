namespace Evaluation.DAL.Dtos.Form;

public class SubFormItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<SubItenList> SubItemLists { get; set; }
}
public class SubItenList
{
    public Guid Id { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public int? OrderNo { get; set; }
}