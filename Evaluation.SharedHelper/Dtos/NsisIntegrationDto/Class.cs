using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class Class : NSISBaseDto
{
    public string? Title { get; set; }
    public string? ClassCode { get; set; }
    public string? ClassType { get; set; }
    public ClassMetaData? MetaData { get; set; }
    //TODO:Need To Add > School, Course, Terms
}
