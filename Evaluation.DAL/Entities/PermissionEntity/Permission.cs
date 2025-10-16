<<<<<<< HEAD:Evaluation.DAL/Entities/PermissionEntity/Permission.cs
﻿using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Base;
=======
﻿using Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;
>>>>>>> 4feb881e9f32991d7c3842ef37c79f5bfee0a8b5:Evaluation.DAL/Entities/Authentication/Permission.cs

namespace Evaluation.DAL.Entities.PermissionEntity;


[Index(nameof(BackendName), IsUnique = true)]
public class Permission : EntityBase
{
    public string BackendName { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? Description { get; set; }
    public ICollection<ControlValidation> ControlValidations { get; set; } = new List<ControlValidation>();
    public ICollection<PagePermission> PagePermissions { get; set; } = new List<PagePermission>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
