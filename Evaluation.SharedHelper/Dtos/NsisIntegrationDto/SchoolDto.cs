using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class SchoolDto : NSISBaseDto
{
    public string? Name { get; set; }
    public string? Identifier { get; set; }
    public SchoolMetaData? MetaData { get; set; }
    //TODO:Need To Add > Parent
}