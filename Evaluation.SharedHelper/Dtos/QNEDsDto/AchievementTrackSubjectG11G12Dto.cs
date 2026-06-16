namespace Evaluation.SharedHelper.Dtos.QNEDsDto;

public class AchievementTrackSubjectG11G12Dto
{
    public int STRM { get; set; }
    public string? Timespan { get; set; }
    public string Institution { get; set; }
    public int Program { get; set; }
    public string? Grade { get; set; }
    public string? Track { get; set; }
    public string? CourseCode { get; set; }
    public string? CourseTitle { get; set; }
    public decimal Grade_NumberOf_Students_NoSev3_And_Present { get; set; }
    public decimal Grade_Achvment_NoSev3_And_Present { get; set; }
    public decimal Grade_Success_NoSev3_And_Present { get; set; }
}
