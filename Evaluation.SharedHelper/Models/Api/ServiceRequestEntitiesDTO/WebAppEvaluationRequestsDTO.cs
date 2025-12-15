using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO
{
    public class WebAppEvaluationRequestsDTO
    {
        public WebAppEvaluationRequestsDTO()
        {
            Data = new List<EvaluationRequestDTO>();
        }
        public List<EvaluationRequestDTO> Data { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalDataCount { get; set; }

        public bool IsRemainingData { get; set; }
    }
}
