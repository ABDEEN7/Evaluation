

namespace Evaluation.SharedHelper.Models.Admin
{
    public class DepartmentDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string RoutingPath { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string DepIcon { get; set; } = null!;

        //Arabic Img
        public string DepImageFileNameAr { get; set; } = null!;
        public string? DepImageFileNameAr_UiFileName { get; set; }
        public string? DepImageFileNameAr_BlobURL { get; set; }


        //English Img
        public string DepImageFileNameEn { get; set; } = null!;
        public string DepImageFileNameEn_UiFileName { get; set; } = null!;
        public string DepImageFileNameEn_BlobURL { get; set; } = null!;
        
        public bool IsNDA { get; set; }
        public string? DescAr { get; set; }
        public string? DescEn { get; set; }
        public int OrderNo { get; set; } = 0;
        public string Name { get; set; } = null!;
        public Guid? WebsiteAttachmentId { get; set; }

        public bool IsEvaluated { get; set; }

    }
}
