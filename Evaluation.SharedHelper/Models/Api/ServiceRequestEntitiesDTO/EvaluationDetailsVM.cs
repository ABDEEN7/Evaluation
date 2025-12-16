using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO
{
	public class EvaluationDetailsVM
	{
		public Guid RequestId { get; set; }

		public string RequestNumber { get; set; } = string.Empty;

		public string EvaluationPlanName { get; set; } = string.Empty;

		public string Status { get; set; } = string.Empty;

		public bool IsCompleted { get; set; }

		public DateTime CreatedOn { get; set; }

		public string CreatedOnDate =>
			CreatedOn.ToString("yyyy-MM-dd");

		public string CreatedOnTime =>
			CreatedOn.ToString("HH:mm");

		//public EvaluationStatisticsVM Statistics { get; set; } = new();

		public ResponseSchools SchoolInfo { get; set; } = new();

		public List<Attachment> Attachments { get; set; } = new();

		public List<EvaluationParty> EvaluationParty { get; set; } = new();

		//public List<EvaluationEvidenceVM> Evidences { get; set; } = new();

		//public List<ClassObservationVM> ClassObservations { get; set; } = new();

		//public List<SchoolTourVM> SchoolTours { get; set; } = new();

		//public List<LabTourVM> LabTours { get; set; } = new();

		//public EvaluationOutputsVM Outputs { get; set; } = new();

		//public VerbalRatingScaleVM VerbalScale { get; set; } = new();

		//public PeriodicReportVM PeriodicReport { get; set; } = new();
	}
}
