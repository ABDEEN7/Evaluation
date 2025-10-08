namespace Evaluation.DAL.Entities.Template;

public class SMSTemplate
{
    public string TitleAr { get; set; }
    public string TitleEn { get; set; }
    public string Messages { get; set; }
    public string BackendName { get; set; }
    public Guid SMSProfileId { get; set; }
    public SMSProfile SMSProfile { get; set; }
}
