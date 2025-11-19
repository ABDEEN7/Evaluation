namespace Evaluation.SharedHelper.Models.Admin
{
    public class ActionFieldTreeDTO
    {
        public Guid ActionId { get; set; }
        public List<ActionFormGroupDTO> FormGroups { get; set; } = new();
    }


    public class ActionFieldFormGroupDTO
    {
        public Guid id { get; set; }
        public Guid? ActionFieldId { get; set; }

        public string text { get; set; } = null!;
        public string value { get; set; } = null!;
        public bool IsEditable { get; set; }
        public Guid parentId { get; set; }
        public string ParentName { get; set; } = null!;
        public ActionStateDTO state { get; set; } = new ActionStateDTO();
        public bool IsActive { get; set; }
    }

    public class ActionStateDTO
    {
        public bool selected { get; set; }
    }

    public class ActionFormGroupDTO
    {
        public Guid id { get; set; }
        public string text { get; set; } = null!;

        public List<ActionFieldFormGroupDTO> children { get; set; } = new();
    }


    public class ActionStepFieldJsonDTO
    {
        public Guid id { get; set; }
        public string text { get; set; } = null!;
        public bool isEditable { get; set; }
        public bool isUpdateOnModule { get; set; }
        public string parentId { get; set; } = null!;
        public string parentName { get; set; } = null!;
        public bool isSelected { get; set; }
        public bool isActive { get; set; }

    }
}
