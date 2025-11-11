
namespace Evaluation.SharedHelper.Models.Admin
{
    public class ServiceStatusDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;

        public string NameEn { get; set; } = null!;

        //public string? BackendName { get; set; }

        public string? ColorCode { get; set; }

        public Guid ServiceId { get; set; }

        public Guid StatusGroupId { get; set; }

        public bool IsInitial { get; set; }

        public bool IsOpen { get; set; }

        //public int? OrderNo { get; set; }
    }
}

