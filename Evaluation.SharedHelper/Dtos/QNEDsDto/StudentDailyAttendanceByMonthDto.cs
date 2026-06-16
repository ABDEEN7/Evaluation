namespace Evaluation.SharedHelper.Dtos.QNEDsDto;

public class StudentDailyAttendanceByMonthDto
{
    public int STRM { get; set; }
    public int Month { get; set; }
    public string? SchoolID { get; set; }
    public int Program { get; set; }
    public string? Grade { get; set; }
    public string? PlanSection { get; set; }
    public int Students { get; set; }
    public string? AttendanceDescription { get; set; }
}
