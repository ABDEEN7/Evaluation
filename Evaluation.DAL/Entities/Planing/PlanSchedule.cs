<<<<<<< HEAD
﻿using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.OrganizationTree;
=======
﻿using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Org;
>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5

namespace Evaluation.DAL.Entities.Planing;

public class PlanSchedule : EntityBase
{
    public int PlanId { get; set; }
    public int SchoolId { get; set; }
    public School? School { get; set; }
    public Plan? Plan { get; set; }
}