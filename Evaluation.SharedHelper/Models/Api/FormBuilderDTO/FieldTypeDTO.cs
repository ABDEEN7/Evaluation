
using Scholarship.SharedHelper.Models.Admin;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FieldTypeDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public int? OrderNo { get; set; }
        public ICollection<FieldDTO> Fields { get; } = new List<FieldDTO>();

    }
}
