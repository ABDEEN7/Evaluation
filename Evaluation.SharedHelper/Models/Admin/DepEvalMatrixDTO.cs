using Evaluation.DAL.Models.PermissionEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Evaluation.SharedHelper.Models.Admin;

public class DepEvalMatrixDTO : EntityBaseDTO
{
    public Guid AcademicYearId { get; set; }
    public Guid? AcademicYear { get; set; }
    public string? BackendName { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public decimal? ItemValue { get; set; }
    public decimal MinValue { get; set; }
    public decimal MaxValue { get; set; }
    public bool RequiredFollowUp { get; set; }
    public int NextFollowUpDays { get; set; }
    public int NextEvaluationDays { get; set; }
}