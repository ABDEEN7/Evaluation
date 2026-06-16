namespace Evaluation.SharedHelper.Dtos.QNEDsDto;

public class YearlyStudentBelow70GradeDto
{
    public int strm { get; set; }
    public string? Institution { get; set; }
    public string? Grade { get; set; }
    public string? CourseCode { get; set; }
    public string? TermCode { get; set; }
    public int Program { get; set; }
    public int No_of_Student_Grading_Assignment { get; set; }
    public decimal No_of_Student_Grading_Assignment_Below_70 { get; set; }
}
