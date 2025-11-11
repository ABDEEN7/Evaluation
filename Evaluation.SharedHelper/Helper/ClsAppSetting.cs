using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Helper
{
    public static class ClsAppSetting
    {
        public static string EvaluationConnectionString { get; set; } = "";
        public static string AzureBlobConnectionString { get; set; } = "";
        public static string TenantId { get; set; } = "";
        public static string ClientSecret { get; set; } = "";
        public static string ClientId { get; set; } = "";
        public static string BlobSasToken { get; set; } = "";
        public static string BlobSasUrl { get; set; } = "";
        public static string BlobEndUrl { get; set; } = "";
        public static string AllowAdminCorsOnly { get; set; } = "";
        public static string AllowWebCorsOnly { get; set; } = "";
        public static string BaseApiUrl { get; set; } = "";
        public static string StudentCertificateUserName { get; set; } = "";
        public static string StudentCertificatePassword { get; set; } = "";
        public static string ValidationURL { get; set; } = "";
        public static string StudentInfoURL { get; set; } = "";
        public static bool IsProduction { get; set; }
        public static string FormJwtConfigKey { get; set; } = "";
        public static string OracleDBConnection { get; set; } = "";
    }
}
