namespace Evaluation.DAL.SystemSetting;

public class SMSProfile
{
    public string BackendName { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsDefault { get; set; }
}
