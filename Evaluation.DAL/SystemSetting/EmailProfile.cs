using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.SystemSetting;

[Index(nameof(BackendName), IsUnique = true)]
public class EmailProfile
{
    public string SenderAddress { get; set; } = null!;
    public string SenderDisplayName { get; set; } = string.Empty;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool EnableSSL { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public bool IsBodyHTML { get; set; }
    public int EmailRequestTimeout { get; set; }
    public string? BackendName { get; set; }
    public bool IsDefault { get; set; }
}
