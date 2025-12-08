
namespace Evaluation.SharedHelper.Models.Admin
{
    public class AcademicYearDTO : EntityBaseDTO
    {

        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Year { get; set; }
        public Guid DepartmentId { get; set; }
        public string Department { get; set; } = null!;
        public bool IsCurrent { get; set; }

    }
}
