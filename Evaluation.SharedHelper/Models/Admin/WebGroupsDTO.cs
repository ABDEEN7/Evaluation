

namespace Evaluation.SharedHelper.Models.Admin
{
    public class WebGroupsDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string RoutingPath { get; set; } = null!;
        public string BackendName { get; set; } = null!;

        public Guid[]? DepWebGroup { get; set; }

    }
}
