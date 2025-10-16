<<<<<<< HEAD
﻿using Evaluation.DAL.Entities.Base;
=======
﻿using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5

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