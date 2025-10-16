using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;

namespace Evaluation.DAL.Entities.Website;

public class Navbar : EntityBase, IAuditLogEntity
{

    public string TitleAr { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public Guid? ParentId { get; set; }
    public Navbar? Parent { get; set; }

    public bool IsInternal { get; set; }

    public string UrlAr { get; set; } = null!;

    public string UrlEn { get; set; } = null!;

    public int? OrderNo { get; set; }

    public string? Target { get; set; }
    public bool IsAuthorized { get; set; }

    public Guid? PermissionId { get; set; }
    public Permission? Permission { get; set; }

}
