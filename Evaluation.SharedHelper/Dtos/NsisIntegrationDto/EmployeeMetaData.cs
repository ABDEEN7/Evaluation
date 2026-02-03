using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class EmployeeMetaData
{
    public string? LmsRole { get; set; }
    public List<string>? GradeLevels { get; set; }
    //public string? Courses { get; set; }
    public string? EnglishName { get; set; }
    public string? Address { get; set; }
}
