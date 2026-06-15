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
        public static string FormJwtExpirationTime { get; set; } = "";
        public static string OracleDBConnection { get; set; } = "";
        public static string NsisBaseURL { get; set; } = "";
        public static string NsisAuthenticationURL { get; set; } = "";
        public static string NsisUsername { get; set; } = "";
        public static string NsisPassword { get; set; } = "";
        public static string NsisGrantType { get; set; } = "";
        public static string NsisSchoolsApi { get; set; } = "";
        public static string NsisClassesApi { get; set; } = "";
        public static string NsisTeachersApi { get; set; } = "";
        public static string NsisStaffApi { get; set; } = "";
        public static string NsisEnrollmentApi { get; set; } = "";
        public static int NsisLimit { get; set; } = 0;
        public static string? NsisStatus { get; set; } = "";
        public static string QNEDSConnection { get; set; } = "";
    }
}
