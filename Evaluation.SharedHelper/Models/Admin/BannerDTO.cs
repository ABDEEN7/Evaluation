
namespace Evaluation.SharedHelper.Models.Admin
{
    public class BannerDTO : EntityBaseDTO
    {


        public string TitleAr { get; set; } = null!;

        public string TitleEn { get; set; } = null!;

        public string? TitleColor { get; set; }

        public string SummaryAr { get; set; } = null!;

        public string SummaryEn { get; set; } = null!;

        public string? SummaryColor { get; set; }

        public string? UrlAr { get; set; }

        public string? UrlEn { get; set; }

        public string? ImgNameAr { get; set; }

        public string? ImgNameAr_UiFileName { get; set; }

        public string? ImgNameAr_BlobURL { get; set; }
        public string? ImgNameAr_FileExt { get; set; }
        public long? ImgNameAr_Size { get; set; }

        public string? ImgNameEn { get; set; }

        public string? ImgNameEn_UiFileName { get; set; }

        public string? ImgNameEn_BlobURL { get; set; }
        public string? ImgNameEn_FileExt { get; set; }
        public long? ImgNameEn_Size { get; set; }

        public string? BtnNameAr { get; set; }

        public string? BtnNameEn { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool ShowAr { get; set; }

        public bool ShowEn { get; set; }
        public string? Target { get; set; }

        public int? OrderNo { get; set; }
    }
}