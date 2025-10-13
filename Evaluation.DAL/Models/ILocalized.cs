using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models
{
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
}
