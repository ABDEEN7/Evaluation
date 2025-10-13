using System;
using System.Collections.Generic;
namespace Evaluation.SharedHelper.Helper
{
    public class RequestInfo
    {

        public string Token { get; set; } = null!;
        public string Lang { get; set; } = null!;
        public int? PageNumber { get; set; }
        public string UserAgent { get; set; } = null!;
        public string UserIp { get; set; } = null!;
        public string Controller { get; set; } = null!;
        public string Action { get; set; } = null!;

        public Dictionary<string, string> AdditionalParameters { get; set; } = new();
    }
}
