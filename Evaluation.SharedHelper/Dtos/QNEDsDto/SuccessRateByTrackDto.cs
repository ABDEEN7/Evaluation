namespace Evaluation.SharedHelper.Dtos.QNEDsDto;

public class SuccessRateByTrackDto
{
    public int STRM { get; set; }
    public string? Timespan { get; set; }
    public string? Institution { get; set; }
    public int Program { get; set; }
    public string? Grade { get; set; }
    public string? Track { get; set; }
    public decimal NotSucceededPercentOfStudent { get; set; }
    public decimal SucceededPercentOfStudent { get; set; }
}
