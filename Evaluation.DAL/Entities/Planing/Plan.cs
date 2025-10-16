<<<<<<< HEAD
﻿using Evaluation.DAL.Entities.Base;
=======
﻿using Evaluation.DAL.Entities.BaseModule;
>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.DepartementEntites;

namespace Evaluation.DAL.Entities.Planing;

public class Plan : EntityBase
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? ExpectedListJson { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public int PlanStatusId { get; set; }
    public PlanStatus? PlanStatus { get; set; }
    public int PlanScheduleId { get; set; }
    public PlanSchedule? PlanSchedule { get; set; }
    public ICollection<ChangeRequest>? ChangeRequests { get; set; }
    public int PlanTypeId { get; set; }
    public PlanType? PlanType { get; set; }

}
