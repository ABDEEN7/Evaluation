using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Master
{
    public class JobTitle : EntityBase
    {
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? HRCode { get; set; }
        public int OrderNo { get; set; } = 999!;

    }
}
