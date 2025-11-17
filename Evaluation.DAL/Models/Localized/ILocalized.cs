namespace Evaluation.DAL.Models.Localized;

public interface ILocalized
{
    string NameEn { get; set; }
    string NameAr { get; set; }
}
public interface ILocalizedFull
{
    public string FullNameAr { get; set; }
    public string FullNameEn { get; set; }
}