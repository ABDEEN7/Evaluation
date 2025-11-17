using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;

namespace Evaluation.DAL.Models.Website;

public  class SiteDocument : EntityBase, IAuditLogEntity
{

    public string? TitleAr { get; set; }

    public string? TitleEn { get; set; }
    public string Skey { get; set; } = null!;
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string FileName { get; set; } = null!;

    public string FileName_UiFileName { get; set; } = null!;

    public string? FileName_BlobURL { get; set; }

    public string FileName_FileExt { get; set; } = null!;

    public long FileName_Size { get; set; }

    public int? ViewCount { get; set; }

   
}
