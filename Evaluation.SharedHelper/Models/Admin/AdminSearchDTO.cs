namespace Evaluation.SharedHelper.Models.Admin
{
    public class AdminSearchDTO
    {
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
        public string Title { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public  Guid? navbarId { get; set; }

        public string? IsActive { get; set; }
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? PageName { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? serviceActionId { get; set; }
        public Guid? currentStatusId { get; set; }
        public Guid? nextStatusId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? AcademicYearId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? OrgClassId { get; set; }
       

    }
}
