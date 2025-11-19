using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Master
{
    public class UserGender : EntityBase
    {
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

    }
}
