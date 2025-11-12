namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FindServiceRequestDetailsDTO
    {
        public string? searchTxt { get; set; }
        public Guid? studentId { get; set; }
        public Guid? universityId { get; set; }
        public Guid? countryId { get; set; }
    }

    public class ServiceRequestDetailsDTO
    {
        public Guid Id { get; set; }
        public string RequestNumber { get; set; }
        public Guid StudentUserId { get; set; }
        public string? StudentUserName { get; set; }
        public string? QID { get; set; }
        //public string? ScholarshipNumber { get; set; }
        //public string? UniversityName { get; set; }
        //public string? CountryName { get; set; }
        public string StudentUserNameAr { get; set; }
        public string StudentUserNameEn { get; set; }
        public Guid ScholarshipId { get; set; }

    }
}
