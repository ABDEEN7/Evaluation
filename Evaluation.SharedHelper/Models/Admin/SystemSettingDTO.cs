


namespace Evaluation.SharedHelper.Models.Admin
{
    public class SystemSettingDTO:EntityBaseDTO
    {
        public Guid? DepartmentId { get; set; }
        public string? Department { get; set; }
        public string? SettingGroup { get; set; }
        public string? SettingKey { get; set; }

        public string? SettingValue { get; set; }

        public string? Description { get; set; }
    }
}
