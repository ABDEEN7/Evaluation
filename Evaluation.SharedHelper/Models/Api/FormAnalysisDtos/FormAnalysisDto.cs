using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormAnalysisDtos
{
	public class FormAnalysisDto
	{
		public Guid RequestId { get; set; }
		public string? RequestNumber { get; set; }

		public Guid ServiceId { get; set; }
		public string? ServiceNameAr { get; set; }
		public string? ServiceNameEn { get; set; }

		public Guid? EvaluationRequestId { get; set; }
		public Guid? EvaluationPartyId { get; set; }

		public List<FormAnalysisObservationDto> Observations { get; set; } = [];
		public List<FormEvalMatrixValueDto> MatrixValues { get; set; }

	}
	public class FormEvalMatrixValueDto
	{
		public Guid Id { get; set; }

		public Guid FormEvalMatrixId { get; set; }

		public string? NameAr { get; set; }

		public string? NameEn { get; set; }

		public decimal MinValue { get; set; }

		public decimal MaxValue { get; set; }

		public decimal ActualMatrixValue { get; set; }

		public string? DescAr { get; set; }

		public string? DescEn { get; set; }

		public int OrderNo { get; set; }

		public bool IsActive { get; set; }
	}
	public class FormAnalysisObservationDto
	{
		public Guid Id { get; set; }
		public string? RequestNumber { get; set; }

		public Guid? SchoolId { get; set; }
		public string? SchoolNameAr { get; set; }
		public string? SchoolNameEn { get; set; }

		public Guid? EducationLevelId { get; set; }
		public string? EducationLevelNameAr { get; set; }
		public string? EducationLevelNameEn { get; set; }

		public Guid? GradeLevelId { get; set; }
		public string? GradeLevelNameAr { get; set; }
		public string? GradeLevelNameEn { get; set; }

		public Guid? SchoolCourseId { get; set; }
		public string? SchoolCourseNameAr { get; set; }
		public string? SchoolCourseNameEn { get; set; }
		public List<FormAnalysisItemDto> Items { get; set; } = [];
	}

	public class FormAnalysisItemDto
	{
		public Guid FormItemId { get; set; }

		public string? ItemNameAr { get; set; }
		public string? ItemNameEn { get; set; }

		public Guid ScopeId { get; set; }
		public string? ScopeNameAr { get; set; }
		public string? ScopeNameEn { get; set; }

		public decimal? ActualValue { get; set; }

		public decimal Min { get; set; }
		public decimal Max { get; set; }
		public decimal Weight { get; set; }

		public string? Note { get; set; }
		public Guid? FormEvalMatrixId { get; set; }
		public string? MatrixNameAr { get; set; }
		public string? MatrixNameEn { get; set; }
	}
}
