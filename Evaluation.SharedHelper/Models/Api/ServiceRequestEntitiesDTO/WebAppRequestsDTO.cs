using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholarship.SharedHelper.Models.Api.ServiceRequestEntitiesDTO
{
    public class WebAppRequestsDTO
    {
        public WebAppRequestsDTO()
        {
            Data = new List<ServiceRequestDTO>();
        }
        public List<ServiceRequestDTO> Data { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalDataCount { get; set; }

        public bool IsRemainingData { get; set; }
    }
}
