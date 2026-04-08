
namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class Course : NSISBaseDto
{
    public string? Title { get; set; }
    public string? CourseCode { get; set; }
    public CourseMetaData? MetaData { get; set; }
    //TODO:Need To Add > Org

}
