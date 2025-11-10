namespace Evaluation.SharedHelper.Models.Admin
{
    public class ActionDTO : EntityBaseDTO
    {
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string BakendName { get; set; }
        public string ActionTypeBackEndKey { get; set; }
        public bool IsConfirmationAction { get; set; }
        public string? ActionTypeId { get; set; }
        public string? ServiceId { get; set; }

        public string Title { get; set; }
        public string ActionTypeType { get; set; }
        public List<TempLateDocDTO> TempLateDoc { get; set; }
        public ActionTypeDTO? ActionType { get; set; }
    }
}
