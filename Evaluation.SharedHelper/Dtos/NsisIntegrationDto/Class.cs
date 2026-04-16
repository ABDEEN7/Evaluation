using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class Class : NSISBaseDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? ClassCode { get; set; }
    public string? ClassType { get; set; }
    public ClassMetaData? MetaData { get; set; }
}