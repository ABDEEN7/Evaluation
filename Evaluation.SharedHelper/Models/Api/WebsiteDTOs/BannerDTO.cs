
namespace Evaluation.SharedHelper.Models.Api.WebsiteDTOs
{
    public class BannerDTO
    {
        public string Title { get; set; }
        public string? TitleColor { get; set; }
        public string Summary { get; set; }
        public string? SummaryColor { get; set; }
        public string? Url { get; set; }
        public string? ImgName { get; set; }
        public string? ImgURL { get; set; }
        public string? ImgExt { get; set; }
        public string? BtnName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Target { get; set; }
        public int? OrderNo { get; set; }
    }
}