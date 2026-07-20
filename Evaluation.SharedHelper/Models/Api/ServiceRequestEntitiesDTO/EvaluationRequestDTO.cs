using Azure;
using Evaluation.DAL.Models.Org;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.LogsDTO;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO
{
	public class EvaluationRequestDTO
	{

		public UserProfileCustomDTO? Applicant { get; set; }
		public ResponseSchools? School { get; set; }

		public string? RequestNumber { get; set; }
		public string? Status { get; set; }
		public string? StatusColor { get; set; }
		public Guid? StatusId { get; set; }
		public bool? StatusISOPen { get; set; }

		public string? Service { get; set; }
		public Guid? ServiceId { get; set; }
		public string? CreateOn { get; set; }
		public string? CreateOnTime { get; set; }
		public DateTime? CreateDate { get; set; }
		public List<FieldValueDTO>? FieldValueDTOs { get; set; }
		public List<FormGroupDTO>? formGroups { get; set; }
		public List<DropDownValueDTO?>? DropDownValues { get; set; }

		public List<AttachementDTO?>? SchAttachments { get; set; }
		public List<AttachementDTO?>? Attachments { get; set; }
		public List<Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs.ActionDTO>? Actions { get; set; }
		public ActionCustomDTO ActionCustom { get; set; }
		public int? ActionCount { get; set; }
		public List<string?>? AssignedUserId { get; set; }
		public Guid? Id { get; set; }
		public string? icon { get; set; }
		[FromForm]
		public IFormFileCollection? Files { get; set; }
		public IList<ActionTransactionLogDTO?>? ActionTransactions { get; set; }
		public string? Mobile { get; set; }
		public string? QID { get; set; }
		public Guid? StudentUserId { get; set; }
		public string? OwnerName { get; set; }
		public bool? StudentIsSpecial { get; set; }
		public string? planNo { get; set; }
		public Guid? PlanId { get; set; }
        public Guid? AcademicYearId { get; set; }

        public string? StudentNationalityId { get; set; }
		public Guid? CountryId { get; set; }
		public Guid? UniversityId { get; set; }
		public bool CanViewFieldHistory { get; set; }
		public bool CanViewAllFieldHistory { get; set; }
		public bool IsOpen { get; set; }
		public string? AssignedTo { get; set; }
		public List<string>? AssignedToIdsList { get; set; }

		public string? PlanName { get; set; }
		public string? EvaluationType { get; set; }

		public Guid OrgTreeId { get; set; }
		public string? OrgTreeName { get; set; }
		public bool IsNdaApprovalPending { get; set; }

		public List<EvaluationPartyDTO> EvaluationParties { get; set; }
		public List<EvaluationRequestAssignmentDto> Assignment { get; set; }

        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string? EvlDateFrom { get; set; }
        public string? EvlDateTo { get; set; }
        public DateOnly? EvaluationDate { get; set; }
        public DateOnly? NextEvaluationDate { get; set; }
        public string? EvaluationResult { get; set; }
    }
}
