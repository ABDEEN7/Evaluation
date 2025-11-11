namespace Evaluation.SharedHelper.Models.Admin
{
    public class ActionTypeDTO : EntityBaseDTO
    {
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string BackEndName { get; set; }
        public int? OrderNo { get; set; }
        public bool? IsRequiredValidation { get; set; }
    }
}
