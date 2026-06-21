using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormAnalysisDtos
{
	public class OutputAnalysisResponseDto
	{
		public List<OutputAnalysisTypeDto> AnalysisTypes { get; set; } = new();
	}

	public class OutputAnalysisTypeDto
	{
		public Guid AnalysisTypeId { get; set; }

		public string AnalysisTypeNameAr { get; set; } = "";
		public string AnalysisTypeNameEn { get; set; } = "";
		public string BackendName { get; set; } = "";

		public OutputAnalysisFinalResultDto? OutputAnalysisFinalResult { get; set; }

		public List<OutputAnalysisDataDto> OutputAnalysisData { get; set; } = new();
	}

	public class OutputAnalysisFinalResultDto
	{
		public Guid Id { get; set; }
		public decimal ActualValue { get; set; }
		public string? Note { get; set; }
		public Guid? FormEvalMatrixValueId { get; set; }
	}

	public class OutputAnalysisDataDto
	{
		public Guid Id { get; set; }
		public int Grade { get; set; }
		public int LastYear { get; set; }
		public int PreviousYear { get; set; }
		public decimal LastYearValue { get; set; }
		public decimal PreviousYearValue { get; set; }
		public decimal Difference { get; set; }
		public string? SubjectCode { get; set; }
		public string? Track { get; set; }
		public decimal ActualValue { get; set; }
		public string? MartixTextValue { get; set; }
		public string? Note { get; set; }
		public int LastYearStudentCount { get; set; }
		public int PreviousYearStudentCount { get; set; }
		public string? TermCode { get; set; }
		public string? DataConfig { get; set; }
	}
}
