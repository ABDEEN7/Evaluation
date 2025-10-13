using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Enums
{
    public static class AppSettings
    {
        public static Guid CreateById;
        public static string DefaultLanguage { get; set; } = null!;
        public static string DateFormat { get; set; } = null!;
    }
}
