using Evaluation.Services.Models.NSISSchool;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Extensions
{
	public static class NSISReaderExtensions
	{
		private static string? SafeString(this DbDataReader r, string col)
		{
			int ord = r.GetOrdinal(col);
			return r.IsDBNull(ord) ? null : r.GetValue(ord)?.ToString();
		}

		private static int SafeInt(this DbDataReader r, string col)
		{
			int ord = r.GetOrdinal(col);

			if (r.IsDBNull(ord))
				return 0;

			return Convert.ToInt32(r.GetValue(ord));
		}

		private static bool YesNo(this DbDataReader r, string col)
		{
			var value = r.SafeString(col);

			return string.Equals(value, "Y", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
		}

		public static NSISSchoolDto ToNSISSchoolDto(this DbDataReader r) => new()
		{
			Institution = r.SafeString("INSTITUTION"),
			Location = r.SafeString("LOCATION"),
			NameEn = r.SafeString("SCHOOL_NAME_ENG"),
			NameAr = r.SafeString("SCHOOL_NAME_ARA"),
			Campus = r.SafeString("CAMPUS"),
			AreaCode = r.SafeString("AREA"),
			AreaNameAr = r.SafeString("AREA_NAME_ARA"),
			AreaNameEn = r.SafeString("AREA_NAME_ENG"),
			SubAreaCode = r.SafeString("SUB_AREA"),
			SubAreaNameAr = r.SafeString("SUB_AREA_NAME_ARA"),
			SubAreaNameEn = r.SafeString("SUB_AREA_NAME_ENG"),
			Address = r.SafeString("ADDRESS_LINE1"),
			Phone = r.SafeString("PHONE"),
			Fax = r.SafeString("FAX"),
			Email = r.SafeString("EMAIL_ADDR"),
			SchoolCategoryCode = r.SafeString("SCHOOL_CATEGORY"),
			SchoolCategoryAr = r.SafeString("SCHOOL_CATEGORY_ARA"),
			SchoolCategoryEn = r.SafeString("SCHOOL_CATEGORY_ENG"),
			GenderCode = r.SafeString("SCHL_GENDER_CD"),
			GenderName = r.SafeString("SCHL_GENDER_DN"),
			HasKG = r.YesNo("KG"),
			HasPrimary = r.YesNo("PRIMARY"),
			HasPreparatory = r.YesNo("PREPARATORY"),
			HasSecondary = r.YesNo("SECONDARY"),
			ModelCode = r.SafeString("SCHL_MODEL_CD"),
			ModelName = r.SafeString("SCHL_MODEL_DN"),
			CohortCode = r.SafeString("SCHL_COHORT_CD"),
			CohortName = r.SafeString("SCHL_COHORT_DN"),
			EmpGenderCode = r.SafeString("SCHL_EMP_GEN_CD"),
			EmpGenderName = r.SafeString("SCHL_EMP_GEN_DN"),
			TypeCode = r.SafeString("SCHL_TYPE_CD"),
			TypeName = r.SafeString("SCHL_TYPE_DN"),
			Latitude = r.SafeString("LATITUDE"),
			Longitude = r.SafeString("LONGITUDE"),
			Url = r.SafeString("URL"),
			OutsCode = r.SafeString("SCHL_OUTS_CD"),
			OutsName = r.SafeString("SCHL_OUTS_DN"),
			ProgramCode = r.SafeString("SCHL_PROGRAM_CD"),
			ProgramName = r.SafeString("SCHL_PROGRAM_DN"),
			SchoolCapacity = r.SafeInt("SCHOOL_CAPACITY"),
			PinNumber = r.SafeString("SCHL_PIN_NUMBER"),
		};

		public static NSISScheduleDto ToNSISScheduleDto(this DbDataReader r) => new()
		{
			Institution = r.SafeString("INSTITUTION"),
			EmplId = r.SafeString("EMPL_ID"),
			Subject = r.SafeString("SUBJECT"),
			AcadPlan = r.SafeString("ACAD_PLAN"),
			PlanSection = r.SafeString("PLAN_SECTION"),
			GroupId = r.SafeString("GROUP_ID"),
			Periods = r.SafeInt("PERIODS"),
			DayCd = r.SafeInt("DAYCD"),
			PeriodBegin = r.IsDBNull(r.GetOrdinal("SC_PRD_BGN_TM")) ? default : Convert.ToDateTime(r.GetValue(r.GetOrdinal("SC_PRD_BGN_TM"))),
			PeriodEnd = r.IsDBNull(r.GetOrdinal("SC_PRD_END_TM")) ? default : Convert.ToDateTime(r.GetValue(r.GetOrdinal("SC_PRD_END_TM"))),
			LocationCode = r.SafeString("LOCATION"),
		};

		public static NSISHomeroomDto ToNSISHomeroomDto(this DbDataReader r) => new()
		{
			Strm = r.SafeString("STRM"),
			Institution = r.SafeString("INSTITUTION"),
			Location = r.SafeString("LOCATION"),
			AcadLevel = r.SafeString("ACAD_LEVEL"),
			AcadPlan = r.SafeString("ACAD_PLAN"),
			PlanSection = r.SafeString("PLAN_SECTION"),
		};

		public static NSISAcadPlanDto ToNSISAcadPlanDto(this DbDataReader r) => new()
		{
			AcadPlan = r.SafeString("ACAD_PLAN"),
			AcadPlanAr = r.SafeString("ACAD_PLAN_ARA"),
			AcadPlanEn = r.SafeString("ACAD_PLAN_ENG"),
			AcadLevel = r.SafeString("ACAD_LEVEL"),
			AcadProg = r.SafeString("ACAD_PROG"),
			AcadProgAr = r.SafeString("ACAD_PROG_ARA"),
			AcadProgEn = r.SafeString("ACAD_PROG_ENG"),
		};
	}
}
