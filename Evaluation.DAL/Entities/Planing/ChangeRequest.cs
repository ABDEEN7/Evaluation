<<<<<<< HEAD
﻿using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Base;
=======
﻿using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequest : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int RequestTypeId { get; set; }
    public ChangeRequestType? ChangeRequestType { get; set; }
    public int PlanId { get; set; }
    public Plan? Plan { get; set; }
    public int RequestedById { get; set; }
    public User? User { get; set; }
    public string? Notes { get; set; }
}
