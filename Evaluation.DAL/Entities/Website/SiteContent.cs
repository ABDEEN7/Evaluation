using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;


namespace Evaluation.DAL.Entities.Website;

public  class SiteContent : EntityBase, IAuditLogEntity
{
    public string TitleAr { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public string DescriptionAr { get; set; } = null!;

    public string DescriptionEn { get; set; } = null!;
    public string Routing { get; set; } = null!;

    public Guid? ParentId { get; set; }
    public SiteContent? Parent { get; set; }
   
    public Guid? NavbarId { get; set; }
    public Navbar? Navbar { get; set; }

    public string? FileNameAr { get; set; }

    public string? FileNameAr_UiFileName { get; set; }

    public string? FileNameAr_BlobURL { get; set; }

    public string? FileNameAr_FileExt { get; set; }

    public long? FileNameAr_Size { get; set; }

    public string? FileNameEn { get; set; }

    public string? FileNameEn_UiFileName { get; set; }

    public string? FileNameEn_BlobURL { get; set; }

    public string? FileNameEn_FileExt { get; set; }

    public long? FileNameEn_Size { get; set; }

    public int? OrderNo { get; set; }

   
  
    
}
