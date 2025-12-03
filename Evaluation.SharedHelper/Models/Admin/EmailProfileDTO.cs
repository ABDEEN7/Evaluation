namespace Evaluation.SharedHelper.Models.Admin
{
    public class EmailProfileDTO : EntityBaseDTO//special for admin usage
    {
        public string SenderAddress { get; set; } = null!;
        public string SenderDisplayName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsBodyHTML { get; set; }
        public bool IsDefault { get; set; }
        public int EmailRequestTimeout { get; set; }
        public string BackendName { get; set; } = null!;

    }
}
