using  Evaluation.DAL.Entities.BaseModule;
﻿using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.DepartementEntites;

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequestType : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public ICollection<ChangeRequest>? ChangeRequests { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
}