using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class StaffDto : NSISBaseDto
{
    public Guid SchoolId { get; set; }
    public string QID { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public Guid JobFunctionID { get; set; }
    public Guid TeachingAssignmentID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phones { get; set; }
    public string Address { get; set; }
}
