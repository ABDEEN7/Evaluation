using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.SharedHelper.Models.Admin;

public class NdaStatusDepartmentDto : EntityBaseDTO
{

    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; } = 0;
    public Guid NdaStatusId { get; set; }
    public Guid DepartmentId { get; set; }

}
