using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO
{
    public class FilterRequestsDTO
    {
        public FilterRequestsDTO()
        {
            StatusesList = new List<Guid?>();
            AssignedToIdForRequestsFilter = new List<string>();
            DynamicDictionary = new Dictionary<string, string>();
            ServiceId = new List<Guid>();
            CountryId = new List<Guid?>();
            UniversityId = new List<Guid?>();
            StudentNationalityId = new List<string>();
        }

        public string? RequestDateFrom { get; set; }
        public string? RequestDateTo { get; set; }
        public string? RequestNo { get; set; }
        public string? ScholarshipNo { get; set; }

        public List<Guid>? ServiceId { get; set; }
        public List<Guid?>? StatusesList { get; set; }
        public List<string>? AssignedToIdForRequestsFilter { get; set; }

        public string? ModuleName { get; set; }
        public string? ModuelName { get; set; } // optional, depending on usage

        public string? Qid { get; set; }
        public string? Mobile { get; set; }
        public int? PageNumber { get; set; }
        public bool? OderByAction { get; set; }

        public Dictionary<string, string>? DynamicDictionary { get; set; }
        public string? StatusTypeId { get; set; }

        public List<string>? StudentNationalityId { get; set; }
        public Guid? StudentUserId { get; set; }
        public List<Guid?>? CountryId { get; set; }
        public List<Guid?>? UniversityId { get; set; }
    }

}
