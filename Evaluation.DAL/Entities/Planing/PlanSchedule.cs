using  Evaluation.DAL.Entities.BaseModule;
﻿using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Org;

namespace Evaluation.DAL.Entities.Planing;

public class PlanSchedule : EntityBase
{
    public int PlanId { get; set; }
    public int SchoolId { get; set; }
    public School? School { get; set; }
    public Plan? Plan { get; set; }
}