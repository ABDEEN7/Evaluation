<<<<<<< HEAD
﻿using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.OrganizationTree;
=======
﻿using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Org;
>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequestDetail : EntityBase
{
    public int ChangeRequestId { get; set; }
    public ChangeRequest ChangeRequest { get; set; } = new();
    public int SchoolId { get; set; }
    public School? School { get; set; }
    //public DateTime ScheduledDate { get; set; }
    public  string Reason { get; set; } = null!;
    public string? EvidenceDocument { get; set; } //URL or file path to evidence document
}